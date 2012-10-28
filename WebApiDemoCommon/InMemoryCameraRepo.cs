using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WebApiDemoCommon.Models;

namespace WebApiDemoCommon
{
    public class InMemoryCameraRepo
    {
        private readonly ICollection<Camera> _cameras;  

        private static readonly Lazy<InMemoryCameraRepo> LazyInstance = new Lazy<InMemoryCameraRepo>(() => new InMemoryCameraRepo());
        public static InMemoryCameraRepo SingletonInstance { get { return LazyInstance.Value; } }

        //Private constructor - only called by Lazy<T>
        private InMemoryCameraRepo()
        {
            _cameras = new Collection<Camera>
                {
                    new Camera{Id =1, Brand = "Nikon", Model = "D4", Price = 47695, Description = "Top model!"},
                    new Camera{Id =2, Brand = "Nikon", Model = "D800E", Price = 23495, Description = "Ultimate sharpness, but Moiré can be problematic with repeating patterns."},
                    new Camera{Id =3, Brand = "Nikon", Model = "F5", Price = 3000, Description = "The best 35mm film camera in history - period!"},
                    new Camera{Id =4, Brand = "Nikon", Model = "D600", Price = 14525, Description = "FX at a reasonable price."}
                };
        }

        //Returns all cameras
        public IEnumerable<Camera> GetCameras()
        {
            return _cameras;
        }

        //Returns single camera
        public Camera GetCamera(int id)
        {
            var camera = _cameras.SingleOrDefault(x => x.Id == id);

            if (camera != null)
            {
                return camera;
            }
            else
            {
                throw new Exception(string.Format("Camera with Id={0} not found.", id));
            }
        }

        
        public void AddCamera(Camera camera)
        {
            if (!CameraExists(camera.Id ?? 0))
            {
                _cameras.Add(camera);
            }  
            else
            {
                throw new Exception(string.Format("Unable to create camera. Camera with Id={0} already exists.", camera.Id));
            }
        }


        public void UpdateCamera(Camera camera)
        {
            var cameraToUpdate = _cameras.SingleOrDefault(x => x.Id == camera.Id);

            if(cameraToUpdate != null)
            {
                cameraToUpdate.Brand = camera.Brand;
                cameraToUpdate.Model = camera.Model;
                cameraToUpdate.Price = camera.Price;
            }
            else
            {
                throw new Exception(string.Format("Unable to update camera. Camera with id={0} not found.", camera.Id));
            }
        }

        public void DeleteCamera(int id)
        {
            if(CameraExists(id))
            {
                _cameras.Remove(_cameras.Single(x => x.Id == id));
            }
            else
            {
                throw new Exception(string.Format("Unable to delete camera. Camera with id={0} not found.", id));
            }
        }

        private bool CameraExists(int id)
        {
            return _cameras.Any(x => x.Id == id);
        }
    }
}
