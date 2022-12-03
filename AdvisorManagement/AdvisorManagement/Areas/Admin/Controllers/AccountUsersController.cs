﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdvisorManagement.Models;

namespace AdvisorManagement.Areas.Admin.Controllers
{
    [Authorize]
    public class AccountUsersController : Controller
    {
        private CP25Team09Entities db = new CP25Team09Entities();

        // GET: Admin/AccountUsers
        public ActionResult Index()
        {
            var accountUser = db.AccountUser.Include(a => a.Role);
            return View(accountUser.ToList());
        }

        // GET: Admin/AccountUsers/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccountUser accountUser = db.AccountUser.Find(id);
            if (accountUser == null)
            {
                return HttpNotFound();
            }
            return View(accountUser);
        }

        // GET: Admin/AccountUsers/Create
        public ActionResult Create()
        {
            ViewBag.id_Role = new SelectList(db.Role, "id", "roleName");
            return View();
        }

        // POST: Admin/AccountUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,user_code,id_Role,username,gender,phone,address,email,dateofbirth,createtime,picture")] AccountUser accountUser)
        {
            if (ModelState.IsValid)
            {
                accountUser.ID = Guid.NewGuid();
                db.AccountUser.Add(accountUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_Role = new SelectList(db.Role, "id", "roleName", accountUser.id_Role);
            return View(accountUser);
        }

        // GET: Admin/AccountUsers/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccountUser accountUser = db.AccountUser.Find(id);
            if (accountUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_Role = new SelectList(db.Role, "id", "roleName", accountUser.id_Role);
            return View(accountUser);
        }

        // POST: Admin/AccountUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
   
        public ActionResult Edit([Bind(Include = "ID,user_code,id_Role,username,gender,phone,address,email,dateofbirth,createtime,picture")] AccountUser accountUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(accountUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_Role = new SelectList(db.Role, "id", "roleName", accountUser.id_Role);
            return View(accountUser);
        }

        // GET: Admin/AccountUsers/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccountUser accountUser = db.AccountUser.Find(id);
            if (accountUser == null)
            {
                return HttpNotFound();
            }
            return View(accountUser);
        }

        // POST: Admin/AccountUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            AccountUser accountUser = db.AccountUser.Find(id);
            db.AccountUser.Remove(accountUser);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
