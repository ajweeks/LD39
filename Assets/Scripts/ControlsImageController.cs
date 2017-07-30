using UnityEngine;

public class ControlsImageController : MonoBehaviour
 {
    public AnimationCurve AnimCurve;
    public float Duration;
    public float Scale;

    private GameObject _controlsImage;
    private float _secondsElapsed;
    private Vector3 _startPos;

	void Start () 
	{
        _controlsImage = GameObject.Find("ControlsSprite");
        _startPos = transform.position;
	}
	

	void Update () 
	{
        _secondsElapsed += Time.deltaTime;

        if (_secondsElapsed >= Duration)
        {
            Destroy(gameObject);
            return;
        }

        float time = _secondsElapsed / Duration;
        _controlsImage.transform.position = new Vector3(0.0f, _startPos.y + AnimCurve.Evaluate(time) * Scale, _startPos.z);
    }
}
