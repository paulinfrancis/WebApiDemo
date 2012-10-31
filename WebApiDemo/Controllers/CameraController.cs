using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiDemo.Security;
using WebApiDemoCommon;
using Camera = WebApiDemoCommon.Models.Camera;

namespace WebApiDemo.Controllers
{
    public class CameraController : ApiController
    {
        // GET api/camera
        public IEnumerable<Camera> Get()
        {
            return InMemoryCameraRepo.SingletonInstance.GetCameras();
        }

        // GET api/camera/5
        public Camera Get(int id)
        {
            try
            {
                return InMemoryCameraRepo.SingletonInstance.GetCamera(id);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage
		        {
			        StatusCode = HttpStatusCode.NotFound, //404
			        Content = new StringContent(ex.Message)
		        });
	        }
        }

        // POST api/camera
        public HttpResponseMessage Post(Camera newCamera)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    InMemoryCameraRepo.SingletonInstance.AddCamera(newCamera);

                    var response = Request.CreateResponse(HttpStatusCode.Created, newCamera);

                    //Add location header entry to newly created resource, so comply with ReST constraints
                    response.Headers.Location = new Uri(Url.Link("DefaultApi", new { controller = "Camera", id = newCamera.Id })); 

                    return response;
                }
                catch (Exception ex)
                {
                    ThrowHttpResponseException(HttpStatusCode.BadRequest, ex); //400
                }               
            }

            //Modelstate is invalid
            var modelValidationErrorResponse = Request.CreateResponse(HttpStatusCode.BadRequest); //400
            modelValidationErrorResponse.Content = new StringContent(ModelState.GetModelErrors());
            return modelValidationErrorResponse;
        }

        // PUT api/camera/5
        public HttpResponseMessage Put(Camera camera)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    InMemoryCameraRepo.SingletonInstance.UpdateCamera(camera);
                    var response = Request.CreateResponse(HttpStatusCode.OK, camera); //Return 200
                    return response;
                }
                catch (Exception ex)
                {
                    ThrowHttpResponseException(HttpStatusCode.NotFound, ex);
                }
            }

            //Modelstate is invalid
            var modelValidationErrorResponse = Request.CreateResponse(HttpStatusCode.BadRequest); //400
            modelValidationErrorResponse.Content = new StringContent(ModelState.GetModelErrors());
            return modelValidationErrorResponse;
        }

        // DELETE api/camera/5
        [BasicAuthentication]
        public void Delete(int id)
        {
            try
            {
                InMemoryCameraRepo.SingletonInstance.DeleteCamera(id);
            }
            catch (Exception ex)
            {
                ThrowHttpResponseException(HttpStatusCode.NotFound, ex);
            }            
        }

        private static void ThrowHttpResponseException(HttpStatusCode statusCode, Exception ex)
        {
            throw new HttpResponseException(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(ex.Message)
            });
        }
    }
}
