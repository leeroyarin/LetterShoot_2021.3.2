using UnityEngine;

namespace Path2D
{
    public class PathCreator: MonoBehaviour
    {
        public Path path;
        [SerializeField]
        internal GlobalDisplaySettings globalDisplaySettings;

        internal void InitializePathCreator()
        {
            this.globalDisplaySettings =GlobalDisplaySettings.Load();
        }
        internal void CreatePath()
        {
            path = new Path(transform.position);
        }
        
  
    }

}