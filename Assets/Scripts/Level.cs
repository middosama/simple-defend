using System.Collections;
using UnityEngine;
using System;
using DG.Tweening;
using PathCreation;
using UnityCommon;
using TMPro;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    public Sprite levelPreview;
    [SerializeField]
    float bonusRatio;

    [SerializeField]
    private Transform objectPool;
    [SerializeField]
    LevelGUI levelGUI;
    [SerializeField]
    Transform worldGUI;

    [SerializeField]
    private InstantBullet instantBulletPrefab;
    [SerializeField]
    private DamageBubble instantBubblePrefab;
    [SerializeField]
    public OptionItem optionItemPrefab;
    [SerializeField]
    public OptionMenu optionMenuPrefab;




    [SerializeField]
    Wave[] waves;
    [SerializeField]
    Transform unitPlaceholdersContainer, enemyContainer;
    UnitPlaceholder[] unitPlaceholders;

    // popup
    [SerializeField]
    float[] speedList;
    //[SerializeField]  /something to visualixze 
    //float[] speedList;
    int speedIndex = 0;
    bool pausing = false;

    float nextWaveTime;

    public UnityEvent OnCoinChange;

    public static Level Instance;

    int currentCreatureCount
    {
        get => creatureList.Count;
    }
    bool isReleasedAll = false;
    List<Creature> creatureList;

    [SerializeField]
    int startCoint = 1000;
    int _currentCoin;
    int currentCoin
    {
        get => _currentCoin;
        set
        {
            _currentCoin = value;
            OnCoinChange.Invoke();
            levelGUI.SetCoin(value);
        }
    }
    [SerializeField]
    readonly int totalHealth = 20;
    int _currentHealth;
    int currentHealth
    {
        get => _currentHealth;
        set
        {
            _currentHealth = value;
            levelGUI.SetHealth(_currentHealth);
            if (_currentHealth <= 0)
            {
                OnLose();
            }
        }
    }

    int currentWaveIndex = 0;
    bool resting = true;

    List<Checkpoint> checkpointList;
    Coroutine waveReleaseLoop;

    private void Start()
    {
        Instance = this;
        Main.ObjectPool = objectPool;
        Main.CurrentGUI = worldGUI.transform;
        InstantBullet.Enqueue(1, instantBulletPrefab);
        DamageBubble.Enqueue(1, instantBubblePrefab);

        DOTween.SetTweensCapacity(125, 70);
        currentCoin = startCoint;
        currentHealth = totalHealth;
        creatureList = new List<Creature>();
        unitPlaceholders = new UnitPlaceholder[unitPlaceholdersContainer.childCount];
        for (int i = 0; i < unitPlaceholdersContainer.childCount; i++)
        {
            unitPlaceholders[i] = unitPlaceholdersContainer.GetChild(i).GetComponent<UnitPlaceholder>();
        }
        checkpointList = new List<Checkpoint>();
        levelGUI.Init(checkpointList);
    }


    void Init()
    {

    }

    private void OnDestroy()
    {
        checkpointList.ForEach(x =>
        {
            x.DestroyTexture();
        });
    }

    public void CoinChange(int changeValue)
    {
        if (changeValue > currentCoin)
            return;
        currentCoin -= changeValue;
    }


    public bool IsEnoughCoin(int changeValue)
    {
        return changeValue < currentCoin;
    }
    public void OnDamage(int changeValue)
    {
        currentHealth -= changeValue;


    }

    public void OnCreatureDisapear(Creature creature)
    {
        Debug.Log("Disapear");
        creatureList.Remove(creature);
        if (isReleasedAll && currentCreatureCount <= 0)
        {
            if (currentHealth > 0)
            {
                OnWin();
            }
            else
            {
                OnLose();
            }
        }
    }

    void OnLose()
    {

        Debug.Log("Lose");
    }

    void OnWin()
    {
        Debug.Log("Win");
        // win, create new record
        if (checkpointList.Count > 0)
        {
            levelGUI.ShowWinConfirmPopup();
        }
        else
        {
            OnWinConfirmed();
        }

        // confirm win
    }

    public void OnWinConfirmed()
    {
        if (currentHealth <= 0 || !isReleasedAll || currentCreatureCount > 0)
            return; // double check

        List<AllyCount> allies = new List<AllyCount>();
        foreach (var unit in unitPlaceholders)
        {
            if (unit.IsAssigned)
            {
                var temp = allies.Find(x => x.allyName == unit.ally.AllyName);
                if (temp != null)
                    temp.count++;
                else
                    allies.Add(new AllyCount(unit.ally.AllyName));
            }
        }
        var record = new LevelRecord() { allies = allies, star = currentHealth == totalHealth ? 3 : currentHealth >= (totalHealth / 2) ? 2 : 1 };
        int diff = LevelSelectController.SaveRecord(record);

        int diffPoint = (int)Math.Round((diff * 0.3f + (allies.Count - diff)) * bonusRatio);
        int starPoint = (int)Math.Round(bonusRatio * record.star);

        levelGUI.ShowWinPanel(diffPoint, starPoint);
        Player.CoinChange(diffPoint + starPoint);
    }

    public void Pause()
    {
        if (!pausing)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = speedList[speedIndex];
        }
        pausing = !pausing;
    }
    public void PassivePause()
    {
        Time.timeScale = 0;
    }
    public void PassiveResume()
    {
        if (!pausing)
            Time.timeScale = speedList[speedIndex];
    }

    public void SpeedUp()
    {
        speedIndex++;
        if (speedIndex >= speedList.Length)
            speedIndex = 0;

        // visualize 

        if (!pausing)
            Time.timeScale = speedList[speedIndex];
    }
    public Checkpoint? CreateCheckpoint()
    {
        if(Player.CreateCheckpoint())
            return SaveCheckpoint();
        return null;
    }

    Checkpoint SaveCheckpoint()
    {

        UnitSnapshot[] snapshotList = new UnitSnapshot[unitPlaceholders.Length];
        for (int i = 0; i < snapshotList.Length; i++)
        {
            snapshotList[i] = unitPlaceholders[i].GetSnapshot();
        }
        Checkpoint checkpoint = new Checkpoint(snapshotList, currentWaveIndex, currentCoin, currentHealth, ScreenCapture.CaptureScreenshotAsTexture());
        checkpointList.Add(checkpoint);
        return checkpoint;
    }

    public void LoadCheckpoint(Checkpoint checkpoint)
    {
        if (!checkpointList.Contains(checkpoint))
            return;
        NextWave(true, checkpoint.waveIndex);
        //currentWaveIndex = checkPoint.waveIndex;
        currentHealth = checkpoint.hp;
        currentCoin = checkpoint.coin;
        for (int i = 0; i < unitPlaceholders.Length; i++)
        {
            unitPlaceholders[i].LoadSnapshot(checkpoint.snapshotList[i]);
        }
        levelGUI.popupGroup.HideAll();
    }

    public void RemoveCheckpoint(Checkpoint checkpoint)
    {
        checkpointList.Remove(checkpoint);
    }



    public void RestartLevel()
    {
        //StopRaid();
        NextWave(true, 0);
        for (int i = 0; i < unitPlaceholders.Length; i++)
        {
            unitPlaceholders[i].LoadSnapshot();
        }
        currentCoin = startCoint;
        currentHealth = totalHealth;
    }


    void StopRaid()
    {
        nextWaveTime = Time.time;
        creatureList.ForEach(x =>
        {
            x.CompletelyDisappeared();
        });
        creatureList.Clear();
        isReleasedAll = false;
    }

    IEnumerator IWaveRelease(int beginIndex)
    {
        for (currentWaveIndex = beginIndex; currentWaveIndex < waves.Length; currentWaveIndex++)
        {

            while (resting)
            {
                if (currentCreatureCount == 0)
                { // still resting
                    // need to burn time here
                    //break;
                    levelGUI.waveTimer.BurnTime(nextWaveTime - Time.time);
                    nextWaveTime = Time.time;
                }
                else if (nextWaveTime < Time.time)
                { // out of time

                    break;
                }
                else
                {
                    levelGUI.waveTimer.SetTimer(nextWaveTime - Time.time);
                }

                yield return null;
            }
            levelGUI.waveTimer.Disappear();
            resting = false;
            Wave currentWave = waves[currentWaveIndex];
            nextWaveTime = Time.time + currentWave.delay;

            float nextRaidTime;
            foreach (var currentRaid in currentWave.raids)
            {
                if (resting)
                {
                    Debug.Log("Break");
                    break;
                }
                nextRaidTime = Time.time + currentRaid.delay;
                var creature = Instantiate(currentRaid.creature, enemyContainer);
                creature.Depath(currentRaid.pathCreator, currentRaid.offset);
                creatureList.Add(creature);
                while (!resting && nextRaidTime > Time.time)
                {
                    yield return null;
                }
                //yield return new WaitForSeconds(currentRaid.delay);
            }
            resting = true;
        }
        isReleasedAll = true;
        waveReleaseLoop = null;
    }

    public void NextWave(bool stopCurrent, int beginWave = 0)
    {
        if (stopCurrent)
        {
            //OptionMenu.BlurAll();
            levelGUI.waveTimer.Appear();
            StopRaid();
        }
        else if (isReleasedAll)
        {
            return;
        }

        resting = stopCurrent;
        if (waveReleaseLoop == null)
        {
            waveReleaseLoop = StartCoroutine(IWaveRelease(beginWave));
        }
        else if (stopCurrent)
        {
            currentWaveIndex = beginWave;
        }
        //nextWaveTime = Time.time;
    }
    public void NextWave()
    {
        if (resting && currentCreatureCount == 0)
            NextWave(false);
    }


    [Serializable]
    struct Wave
    {
        [SerializeField]
        public Raid[] raids;
        [SerializeField]
        public float delay;
    }

    [Serializable]
    struct Raid
    {
        [SerializeField]
        public Creature creature;
        [SerializeField]
        public PathCreator pathCreator;
        [SerializeField]
        public float offset;
        [SerializeField]
        public float delay;
    }

    public struct Checkpoint
    {
        public UnitSnapshot[] snapshotList { get; private set; }
        public int waveIndex { get; private set; }
        public int coin { get; private set; }
        public int hp { get; private set; }
        public Sprite screenshot { get; private set; }
        public Checkpoint(UnitSnapshot[] snapshotList, int waveIndex, int coin, int hp, Texture2D screenshot)
        {
            this.snapshotList = snapshotList;
            this.waveIndex = waveIndex;
            this.coin = coin;
            this.hp = hp;
            this.screenshot = Sprite.Create(screenshot, new Rect(0, 0, screenshot.width, screenshot.height), Vector2.zero);
        }

        public void DestroyTexture()
        {
            Destroy(screenshot.texture);
        }
    }


    public void BackToZone()
    {
        Main.LoadScene("LevelSelectScene");
    }
}
