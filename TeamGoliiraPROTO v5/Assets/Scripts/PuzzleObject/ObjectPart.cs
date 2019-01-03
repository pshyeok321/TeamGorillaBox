using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectData {

    public class ObjectPart : MonoBehaviour {

        public ColorType colorType;
        public RotateAngle rotateAngle;

        public bool isInput = false;

        [Range(0,3)]public int key;

        public Vector3 Position {
            get {
                return position;
            }
            set {
                position = transform.position;
            }
        }
        private Vector3 position;

        // Use this for initialization
        void Awake() {
            position = transform.position;

        }

        // Update is called once per frame
        void Update() {

        }
    }
}