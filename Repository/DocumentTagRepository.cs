using DataModels.Entities;
using DataModels.VM.Document;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Repository
{
    public class DocumentTagRepository : IDocumentTagRepository
    {
        private MyContext _myContext;

        public List<DocumentTagVM> List()
        {
            using (_myContext = new MyContext())
            {
                List<DocumentTagVM> listTags = _myContext.DocumentTags.Where(p => p.IsActive == true && p.IsDeleted == false
                                                ).ToList().Select(p =>
                                                new DocumentTagVM
                                                {
                                                    Id = p.Id,
                                                    TagName = p.TagName,

                                                }).ToList();


                return listTags;
            }
        }

       public List<DocumentTagVM> ListByCondition(Expression<Func<DocumentTag, bool>> predicate)
        {
            using (_myContext = new MyContext())
            {
                List<DocumentTagVM> listTags = _myContext.DocumentTags.Where(predicate).ToList().Select(p =>
                                                new DocumentTagVM
                                                {
                                                    Id = p.Id,
                                                    TagName = p.TagName,

                                                }).ToList();


                return listTags;
            }
        }

        public DocumentTagVM FindByCondition(Expression<Func<DocumentTag, bool>> predicate)
        {
            using (_myContext = new MyContext())
            {
                DocumentTagVM documentTag = _myContext.DocumentTags.Where(predicate).ToList().Select(p =>
                                                new DocumentTagVM
                                                {
                                                    Id = p.Id,
                                                    TagName = p.TagName,

                                                }).FirstOrDefault();


                return documentTag;
            }
        }

        public DocumentTag Create(DocumentTag documentTag)
        {
            using (_myContext = new MyContext())
            {
                _myContext.DocumentTags.Add(documentTag);
                _myContext.SaveChanges();

                return documentTag;
            }
        }

        public List<DocumentTag> Create(List<DocumentTag> documentTagsList)
        {
            using (_myContext = new MyContext())
            {
                _myContext.DocumentTags.AddRange(documentTagsList);

                return documentTagsList;
            }
        }
    }
}
