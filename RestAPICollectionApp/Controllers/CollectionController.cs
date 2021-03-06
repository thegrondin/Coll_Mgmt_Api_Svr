﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Description;
using Helpers;
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

        // GET: api/collection/3/Items
        [Route("api/collection/{collectionId}/Items")]
        public IQueryable<Item> GetItemsFromCollection(int collectionId)
        {
            return db.Items.Where(x => x.CollectionModelId == collectionId);
        }

        [Route("api/collection/{collectionId}/Items")]
        public dynamic GetItemsFromCollection(int collectionId, [FromUri] string fields)
        {
            return db.Items.Where(x => x.CollectionModelId == collectionId).AsEnumerable().GetFieldsFromObjectList(fields);
        }

        // GET: api/collection/4/items/34
        [Route("api/collection/{collectionId}/Items/{itemId}")]
        public IHttpActionResult GetItem(int collectionId, int itemId)
        {
            Item selectedItem = db.Items.Where(x => x.CollectionModelId == collectionId).Where(y => y.ItemId == itemId).FirstOrDefault();

            if (selectedItem == null)
            {
                return NotFound();
            }

            return Ok(selectedItem);
        }

        [Route("api/collection/{collectionId}/Items/{ItemId}")]
        public IHttpActionResult GetItem(int collectionId, int itemId, [FromUri] string fields)
        {
            Item selecteddItem = db.Items.Where(x => x.CollectionModelId == collectionId).Where(y => y.ItemId == itemId).FirstOrDefault();

            if (selecteddItem == null)
                return NotFound();

            return Ok(selecteddItem.GetFieldsFromModel(fields));
        }

        // POST: api/collection/4/items
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

        // DELETE: api/collection/4/items
        [Route("api/collection/{collectionId}/Items/{itemId}")]
        public IHttpActionResult DeleteItem(int collectionId, int itemId)
        {
            Item item = db.Items.Find(itemId);

            if (item == null)
                return NotFound();

            db.Items.Remove(item);
            db.SaveChanges();

            return Ok(item);
        }

        public IEnumerable<object> GetCollections()
        {
            return Collections;
        }


        // GET: api/Collection
        public dynamic GetCollections([FromUri] string fields)
        {
            return db.Collections.AsEnumerable().GetFieldsFromObjectList(fields);
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

        public IHttpActionResult GetCollectionModel(int id, [FromUri] string fields)
        {
            CollectionModel collectionModel = db.Collections.Find(id);
            if (collectionModel == null)
                return NotFound();

            return Ok(collectionModel.GetFieldsFromModel(fields));
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

        private bool ItemExists(int id)
        {
            return db.Items.Count(e => e.ItemId == id) > 0;
        }

        private bool CollectionModelExists(int id)
        {
            return db.Collections.Count(e => e.CollectionModelId == id) > 0;
        }
    }
}