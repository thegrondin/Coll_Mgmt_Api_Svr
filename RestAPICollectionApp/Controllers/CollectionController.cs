using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using RestAPICollectionApp.Models;

namespace RestAPICollectionApp.Controllers
{
    public class CollectionController : ApiController
    {

        private CollectionContext db = new CollectionContext();

        private IEnumerable<object> Collections { get; set; }
        public CollectionController()
        {
            Collections = db.Collections.Include("Items").Select(i =>
                new { i.CollectionModelId, i.Name, i.Description, i.Items }).ToList();
        }

        [Route("api/collection/{collectionId}/Items")]
        public IQueryable<Item> GetItemsFromCollection(int collectionId)
        {
            return db.Items.Where(x => x.CollectionModelId == collectionId);
        }

        [Route("api/collection/{collectionId}/Item/{itemId}")]
        public IHttpActionResult GetItem(int collectionId, int itemId)
        {
            Item selectedItem = db.Items.Where(x => x.CollectionModelId == collectionId).Where(y => y.ItemId == itemId).FirstOrDefault();

            if (selectedItem == null)
                return NotFound();

            return Ok(selectedItem);
        }

        [Route("api/collection/{collectionId}/Items")]
        public IHttpActionResult PostItem(int collectionId, Item item)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            item.CollectionModelId = collectionId;

            db.Items.Add(item);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = item.CollectionModelId }, item);
        }



        // GET: api/Collection
        public IEnumerable<object> GetCollections()
        {
            return Collections;   
        }


        // GET: api/Collection/5
        [ResponseType(typeof(CollectionModel))]
        public IHttpActionResult GetCollectionModel(int id)
        {
            CollectionModel collectionModel = db.Collections.Find(id);
            if (collectionModel == null)
            {
                return NotFound();
            }

            return Ok(collectionModel);
        }

        // PUT: api/Collection/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCollectionModel(int id, CollectionModel collectionModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != collectionModel.CollectionModelId)
            {
                return BadRequest();
            }

            db.Entry(collectionModel).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CollectionModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Collection
        [ResponseType(typeof(CollectionModel))]
        public IHttpActionResult PostCollectionModel(CollectionModel collectionModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Collections.Add(collectionModel);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = collectionModel.CollectionModelId }, collectionModel);
        }

        // DELETE: api/Collection/5
        [ResponseType(typeof(CollectionModel))]
        public IHttpActionResult DeleteCollectionModel(int id)
        {
            CollectionModel collectionModel = db.Collections.Find(id);
            if (collectionModel == null)
            {
                return NotFound();
            }

            db.Collections.Remove(collectionModel);
            db.SaveChanges();

            return Ok(collectionModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CollectionModelExists(int id)
        {
            return db.Collections.Count(e => e.CollectionModelId == id) > 0;
        }
    }
}