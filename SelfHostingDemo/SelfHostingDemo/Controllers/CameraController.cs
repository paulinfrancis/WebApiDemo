﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiDemoCommon;
using WebApiDemoCommon.Models;

namespace SelfHostingDemo.Controllers
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

                    response.Headers.Location = new Uri(location); //Set location, so that the client knows where to find the newly created resource

                    return response;
                }
                catch (Exception ex)
                {
                    ThrowHttpResponseException(HttpStatusCode.BadRequest, ex); //400
                }               
            }

            var modelValidationErrorResponse = Request.CreateResponse(HttpStatusCode.BadRequest);
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

            var modelValidationErrorResponse = Request.CreateResponse(HttpStatusCode.BadRequest);
            modelValidationErrorResponse.Content = new StringContent(ModelState.GetModelErrors());
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
