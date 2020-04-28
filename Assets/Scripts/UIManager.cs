using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartLevel;
    [SerializeField]
    private Text _backToMenu;
    private Player _player;
    [SerializeField]
    private Image _livesImage;
    [SerializeField]
    private Sprite[] _liveSprites;
    private GameManager _gm;
    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: ";
        _player = GameObject.Find("Player").GetComponent<Player>();
        if(_player == null){
            Debug.LogError("player doesn't exist");
        }
        _gameOverText.gameObject.SetActive(false);
        _backToMenu.gameObject.SetActive(false);
        _gm = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if(_gm == null){
            Debug.LogError("GameManager Null");
        }

    }

    public void UpdateScoreText(int score)
    {
        _scoreText.text = "Score: " + score;
    }
    public void UpdateLives(int currentLives)
    {
        //display impage Sprite, give it a new one based on currentLives param.
        _livesImage.sprite = _liveSprites[currentLives];
    }
    public void GameOverDisplay(){
        _gm.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartLevel.gameObject.SetActive(true);
        _backToMenu.gameObject.SetActive(true);
        StartCoroutine(FlickerTextAlt());
    }
    IEnumerator FlickerText(GameObject g){
        while (true)
        {
            //set inactive
            g.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            g.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }
    //alternate activation of the Game Over Text
    IEnumerator FlickerTextAlt(){
        while (true)
        {
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
