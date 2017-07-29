using UnityEngine;

public class CameraController : MonoBehaviour
 {
    public float MaxOffset;

    private float _durationRemaining;
    private Vector3 _shakeOffset;

    private Camera _camera;
    private Vector3 _cameraStartingPosition;

    private float _shakeDuration;
    private float _xIntensity;
    private float _yIntensity;

    void Start()
    {
        _camera = GetComponent<Camera>();
        _cameraStartingPosition = _camera.transform.position;
    }

    void Update()
    {
        if (_durationRemaining > 0.0f)
        {
            _durationRemaining -= Time.deltaTime;
            _durationRemaining = Mathf.Max(0.0f, _durationRemaining);

            if (_durationRemaining == 0.0f)
            {
                _shakeOffset = Vector3.zero;
                _camera.transform.position = _cameraStartingPosition;
            }
            else
            {
                float durationScale = _durationRemaining / _shakeDuration;
                durationScale = Mathf.Clamp01(durationScale);

                _shakeOffset +=
                    new Vector3(
                        Random.Range(-_xIntensity, _xIntensity),
                        0.0f,
                        Random.Range(-_yIntensity, _yIntensity));
                _shakeOffset *= durationScale;

                _shakeOffset.x = Mathf.Clamp(_shakeOffset.x, -MaxOffset, MaxOffset);
                _shakeOffset.z = Mathf.Clamp(_shakeOffset.z, -MaxOffset, MaxOffset);

                _camera.transform.position = _cameraStartingPosition + _shakeOffset;
            }
        }
    }

    public void Shake(float xIntensity, float yIntensity, float duration)
    {
        _xIntensity = xIntensity;
        _yIntensity = yIntensity;
        _durationRemaining = duration;
        _shakeOffset = Vector3.zero;
    }
}
