using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiDemo.Models;
using WebApiDemo.Security;
using WebApiDemoCommon;
using Camera = WebApiDemoCommon.Models.Camera;

namespace WebApiDemo.Controllers
{
    [BasicAuthentication]
    public class SecureCameraController : ApiController
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
			        StatusCode = HttpStatusCode.NotFound,
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

                    var location = string.Format("{0}{1}{2}", Url.Request.RequestUri.OriginalString, "/", newCamera.Id);

                    #region
                    //TODO: Find out why route is not found
                    //response.Headers.Location = new Uri(Url.Link("DefaultApi", new { controller = "Camera", id = newCamera.Id }));
                    #endregion

                    response.Headers.Location = new Uri(location); //Set location, so that the client knows where to find the newly created resource

                    return response;
                }
                catch (Exception ex)
                {
                    ThrowHttpResponseException(HttpStatusCode.BadRequest, ex); //400
                }               
            }

            //Modelstate is invalid
            var modelValidationErrorResponse = Request.CreateResponse(HttpStatusCode.BadRequest);
            modelValidationErrorResponse.Content = new StringContent(ValidationHelper.GetModelErrors(ModelState));
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
            var modelValidationErrorResponse = Request.CreateResponse(HttpStatusCode.BadRequest);
            modelValidationErrorResponse.Content = new StringContent(ValidationHelper.GetModelErrors(ModelState));
            return modelValidationErrorResponse;
        }

        // DELETE api/camera/5
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
