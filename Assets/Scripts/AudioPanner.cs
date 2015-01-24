using AgonyBartender;
using UnityEngine;
using System.Collections;

namespace AgonyBartender
{

    public class AudioPanner : MonoBehaviour
    {
        private BarManager _barManager;
        private Transform _barStool;
        private AudioSource _source;

        public float VolumeMultipler = 0.8f;
        public float VolumePow = 0.5f;

        public void Start()
        {
            _source = GetComponent<AudioSource>();
            _barManager = transform.GetComponentInParent<BarManager>();
            _barStool = transform;
            while (_barStool.parent != _barManager.transform)
                _barStool = _barStool.parent;
        }

        public void Update()
        {
            int offset = _barManager.GetStoolOffsetFromCurrent(_barStool);

            _source.volume = Mathf.Pow(VolumePow, Mathf.Abs(offset)) * VolumeMultipler;
            _source.pan = (offset < 0) ? -1 : (offset > 0) ? 1 : 0;
        }
    }

}