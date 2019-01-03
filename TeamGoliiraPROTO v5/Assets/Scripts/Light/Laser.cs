using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectData;

namespace Lasers {

    public class Laser : MonoBehaviour {

        #region Public variable 
        public Transform initDirection;
        public GameObject source;
        #endregion

        public ColorType colorType;

        private float _moveSpeed;
        private float _time;
        private float _maxTime;

        private Color _color = Color.red;
        private LineRenderer lineRenderer;
        private LaserSource laserSource;

        private Vector3 direction;

        int layermask;
        public bool recreate = false;
        public bool isReprojection = false;
        bool projection = true;


        void Start() {
            lineRenderer = GetComponent<LineRenderer>();
            laserSource = source.transform.GetComponent<LaserSource>();
            layermask = (1 << 10) + (1 << 11);
        }

        void FixedUpdate() {
            if (isReprojection) {
                ReProjection();

                isReprojection = false;
            }

            if(projection)
                LaserProjection();

            if (!recreate) projection = true;
        }

        void LaserProjection() {
            direction =  initDirection.position - transform.position ;

            Ray ray = new Ray();
            ray.origin = this.transform.position;
            ray.direction = direction;

            RaycastHit hit;

            if(Physics.Raycast(ray.origin, ray.direction, out hit, 150, layermask)) {
                                
                ObjectInfo hitInfo = hit.transform.gameObject.GetComponentInParent<ObjectInfo>();

                if (hitInfo != null) {
                    ObjectType hitType = hitInfo.objectType;
                    lineRenderer.SetPosition(0, this.transform.position);
                    lineRenderer.SetPosition(1, hit.point);


                    if (hitType == ObjectType.Reflector) {
                        if (!recreate) {
                            RotateAngle angle = hitInfo.rotateAngle;
                            GameObject newLight = Instantiate(transform.parent.gameObject, hit.point, Quaternion.identity);
                            newLight.transform.parent = this.transform;
                            LaserSource.laserList.Add(newLight);
                            Laser newLaser = newLight.gameObject.GetComponentInChildren<Laser>();
                            newLaser.initDirection.localPosition = SetDirection(angle);
                            this.recreate = true;
                        }
                    }

                    if (hitType == ObjectType.Dispersant) {
                        if (!recreate) {
                            int hitKey = hit.transform.gameObject.GetComponent<ObjectPart>().key;
                            GameObject[] newLight = new GameObject[4];
                            for (int i = 0; i < 4; i++) {
                                if (i == hitKey)
                                    continue;
                                ObjectPart hitPart = hitInfo.objectParts[i];
                                RotateAngle angle = hitPart.rotateAngle;
                                Vector3 newPos = hitPart.Position;
                                newLight[i] = Instantiate(transform.parent.gameObject, newPos, Quaternion.identity);
                                Laser newLaser = newLight[i].gameObject.GetComponentInChildren<Laser>();
                                newLaser.initDirection.localPosition = SetDirection(angle);
                            }

                            for (int i = 0; i < 4; i++) {
                                if (newLight[i] == null)
                                    continue;
                                newLight[i].transform.parent = this.transform;
                                LaserSource.laserList.Add(newLight[i]);
                            }

                            this.recreate = true;
                        }
                    }

                    if(hitType == ObjectType.Transmission) {
                        if (!recreate) {
                            int hitKey = hit.transform.gameObject.GetComponent<ObjectPart>().key;
                            GameObject newLight;
                            if(hitKey == 0) {
                                ObjectPart hitPart = hitInfo.objectParts[1];
                                RotateAngle angle = hitPart.rotateAngle;
                                Vector3 newPos = hitPart.Position;
                                newLight = Instantiate(transform.parent.gameObject, newPos, Quaternion.identity);
                                newLight.transform.parent = this.transform;
                                Laser newLaser = newLight.gameObject.GetComponentInChildren<Laser>();
                                newLaser.initDirection.localPosition = SetDirection(angle);
                            }
                            else if (hitKey == 1) {
                                ObjectPart hitPart = hitInfo.objectParts[0];
                                Debug.Log(hitPart.transform.parent.name);
                                RotateAngle angle = hitPart.rotateAngle;
                                Vector3 newPos = hitPart.Position;
                                newLight = Instantiate(transform.parent.gameObject, newPos, Quaternion.identity);
                                newLight.transform.parent = this.transform;
                                Laser newLaser = newLight.gameObject.GetComponentInChildren<Laser>();
                                newLaser.initDirection.localPosition = SetDirection(angle);
                            }
                        }

                        this.recreate = true;
                    }

                    if(hitType == ObjectType.Absorber) {
                        hitInfo.doors.SetTrigger("Openning");
                    }

                    if (hitType == ObjectType.Wall) {
                        laserSource.isUpdate = true;
                        this.recreate = true;
                    }
                }
            }

            projection = false;
        }

        void ReProjection() {
            for(int i=0; i<LaserSource.laserList.Count; i++) {
                Destroy(LaserSource.laserList[i].gameObject, 0);
            }

            LaserSource.laserList.Clear();

            recreate = false;
        }

        Vector3 SetDirection(RotateAngle ra) {
            Vector3 temp = Vector3.zero;
            switch (ra) {
                case RotateAngle.Right:
                    temp = new Vector3(1, 0, 0);
                    break;
                case RotateAngle.RightDown:
                    temp = new Vector3(1, 0, -1);
                    break;
                case RotateAngle.Down:
                    temp = new Vector3(0, 0, -1);
                    break;
                case RotateAngle.LeftDown:
                    temp = new Vector3(-1, 0, -1);
                    break;
                case RotateAngle.Left:
                    temp = new Vector3(-1, 0, 0);
                    break;
                case RotateAngle.LeftTop:
                    temp = new Vector3(-1, 0, 1);
                    break;
                case RotateAngle.Top:
                    temp = new Vector3(0, 0, 1);
                    break;
                case RotateAngle.RightTop:
                    temp = new Vector3(1, 0, 1);
                    break;
            }

            return temp;
        }

        void OnDrawGizmos() {
            direction = initDirection.position - transform.position;
            Debug.DrawRay(this.transform.position, direction, Color.red);
            
        }
    }


}