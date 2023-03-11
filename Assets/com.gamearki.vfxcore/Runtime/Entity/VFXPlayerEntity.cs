using UnityEngine;

namespace GameArki.VFX {

    public class VFXPlayerEntity {

        static int autoIncreaseID;
        static object lockObj = new object();

        int vfxID;
        public int VFXID => vfxID;

        ParticleSystem psMain;
        ParticleSystem[] psAll;
        public bool IsLoop => psMain.main.loop;
        public bool IsMainPlaying => psMain.isPlaying;

        string name;
        public string Name => name;
        public void SetName(string v) => name = v;

        bool isManualTick;
        public bool IsManualTick => isManualTick;
        public void SetManualTick(bool v) => isManualTick = v;

        int tickCount;
        public int TickCount => tickCount;
        public void SetTickCount(int v) => tickCount = v;
        public void ReduceTickCount() => tickCount--;

        GameObject vfxGo;

        public VFXPlayerEntity() {
            lock (lockObj) {
                autoIncreaseID++;
                vfxID = autoIncreaseID;
            }
        }

        public void Init() {
        }

        public void TearDown() {
            GameObject.Destroy(vfxGo);
        }

        public void Reset() {
            vfxGo = null;
            tickCount = 0;
            isManualTick = false;
        }

        public void Play() {
            if (psAll != null) {
                foreach (var ps in psAll) {
                    if (ps == null) continue;
                    ps.Play();
                }
            }
            vfxGo.SetActive(true);
        }

        public void Play(bool isLoop) {
            var main = psMain.main;
            main.loop = isLoop;
            if (psAll != null) {
                foreach (var ps in psAll) {
                    if (ps == null) continue;
                    ps.Play();
                }
            }
            vfxGo.SetActive(true);
        }

        public void StopAll() {
            if (psAll != null) {
                foreach (var ps in psAll) {
                    if (ps == null) continue;
                    ps.Stop();
                }
            }
            vfxGo.SetActive(false);
        }


        public void SetParent(Transform parent) {
            if (parent == null) {
                return;
            }

            vfxGo.transform.SetParent(parent, false);
            vfxGo.transform.rotation = parent.rotation;
        }

        public void SetVFXGo(GameObject vfxGo) {
            UnityEngine.Object.DontDestroyOnLoad(vfxGo);
            this.vfxGo = vfxGo;
            this.psMain = vfxGo.GetComponentInChildren<ParticleSystem>();
            this.psAll = vfxGo.GetComponentsInChildren<ParticleSystem>();
        }

        public bool IsAllStopped() {
            for (int i = 0; i < psAll.Length; i++) {
                var ps = psAll[i];
                if (ps == null) continue;

                if (ps.isPlaying) {
                    return false;
                }
            }

            return true;
        }

    }

}