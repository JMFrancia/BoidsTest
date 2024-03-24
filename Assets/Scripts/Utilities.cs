using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

/*
 *  Generally useful tools + functions
 */
public static class Utilities
{
    class StaticMB : MonoBehaviour { }

    static StaticMB _staticMB;
    static AsyncOperation _sceneLoadOp;
    static Coroutine _sceneLoadCoroutine;

    public static float GetScreenWidth() {
        return Camera.main.aspect * GetScreenHeight();
    }

    public static float GetScreenHeight() {
        return Camera.main.orthographicSize * 2;
    }

    public static void ColorIndividualTMProWord(TMPro.TextMeshProUGUI text, string word, Color color) {
        text.text = text.text.Replace(word, $"{word.AddColor(color)}");
    }

    public static int Mod(int x, int m)
    {
        int r = x % m;
        return r < 0 ? r + m : r;
    }

    public static int ClosestMultiple(int n, int x)
    {
        if (x > n)
            return x;
        n = n + x / 2;
        n = n - (n % x);
        return n;
    }

    public static void LoadScene(string sceneName, bool debugLogStatus = false) {
        LoadScene(sceneName, true, debugLogStatus);
    }

    public static void LoadSceneAsyncronously(string sceneName, bool debugLogLoadStatus = false) {
        LoadScene(sceneName, false, debugLogLoadStatus);
    }

    public static void ActivateLoadedScene() {
        if (_sceneLoadOp == null) {
            Debug.LogError("Attempted to call Utilities.ActivateLoadedScene() before Utilities.LoadSceneAsyncronously()");
            return;
        }
        _sceneLoadOp.allowSceneActivation = true;
    }

    public static void CancelSceneLoad()
    {
        if (_sceneLoadCoroutine != null && _staticMB != null)
        {
            _staticMB.StopCoroutine(_sceneLoadCoroutine);
            _sceneLoadOp.allowSceneActivation = false;
        }
    }

    static StaticMB CreateStaticMB(string name) {
        GameObject obj = new GameObject(name);
        return obj.AddComponent<StaticMB>();
    }

    static IEnumerator SceneLoadCoroutine(string sceneName, bool loadOnComplete, bool debugLogLoadStatus) {
        CancelSceneLoad();
        _sceneLoadOp = SceneManager.LoadSceneAsync(sceneName);
        _sceneLoadOp.allowSceneActivation = loadOnComplete;
        while (!_sceneLoadOp.isDone)
        {
            if (debugLogLoadStatus) {
                Debug.Log($"{sceneName} scene loading ... {_sceneLoadOp.progress * 100}%");
            }
            yield return null;
        }
    }

    static void LoadScene(string sceneName, bool loadOnComplete = false, bool debugLogLoadStatus = false)
    {
        if (_staticMB == null)
        {
            _staticMB = CreateStaticMB("SceneLoader");
        }
        _sceneLoadCoroutine = _staticMB.StartCoroutine(SceneLoadCoroutine(sceneName, loadOnComplete, debugLogLoadStatus));
    }

    //TODO: Use 'in' for c, return void
    public static void SetColorAlpha(ref Color c, float alpha)
    {
        Color result = c;
        result.a = alpha;
        c = result;
    }

    public static int[] GetNonRepeatingRandomInts(int min, int max, int total)
    {
        int[] result;
        if (total == 1)
        {
            result = new int[] { UnityEngine.Random.Range(min, max) };
        }
        else
        {
            System.Random rnd = new System.Random();
            result = Enumerable.Range(min, max).OrderBy(x => rnd.Next()).Take(total).ToArray();
        }
        return result;
    }
}

public static class StringExtensions
{
    public static string AddColor(this string text, Color col) => $"<color={ColorHexFromUnityColor(col)}>{text}</color>";
    public static string ColorHexFromUnityColor(this Color unityColor) => $"#{ColorUtility.ToHtmlStringRGBA(unityColor)}";
}
