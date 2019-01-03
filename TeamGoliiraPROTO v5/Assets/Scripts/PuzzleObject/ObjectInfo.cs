using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectData {

    public class ObjectInfo : MonoBehaviour {

        public ObjectType objectType;
        public RotateAngle rotateAngle;

        public ObjectPart[] objectParts;

        public List<Transform> objectPositions = new List<Transform>();

        public Animator doors;

        void Start() {
            for (int i = 0; i < objectParts.Length; i++) {
                objectPositions.Add(objectParts[i].GetComponent<Transform>());
            }

            for (int i = 0; i < objectPositions.Count; i++) { }
                //Debug.Log(objectPositions[i]);
        }

    }

    public enum ObjectType {
        Reflector = 0,      // 반사
        Dispersant,         // 분산
        Transformer,        // 변화
        Transmission,       // 전송
        Absorber,           // 흡수
        Illuminant,         // 발광
        Wall                // 벽
    }

    // 오브젝트 중심을 기준으로
    public enum RotateAngle {
        Right = 0,
        RightDown,
        Down,
        LeftDown,
        Left,
        LeftTop,
        Top,
        RightTop
    }

    // 분산체, 또는 변경
    // 구체적인 색은 화요일 지정
    public enum ColorType {
        AA = 0,
        BB,
        CC,
        DD
    }


}
