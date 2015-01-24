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

        public float VolumeDropPerOffset = 0.2f;

        public void Awake()
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

            _source.volume = 1 - Mathf.Abs(offset)*VolumeDropPerOffset;
            _source.pan = (offset < 0) ? -1 : (offset > 0) ? 1 : 0;
        }
    }

}