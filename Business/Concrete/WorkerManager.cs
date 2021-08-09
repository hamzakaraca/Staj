using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{

    public class WorkerManager : IWorkerService
    {
        IWorkerDal _workerDal;

        public WorkerManager(IWorkerDal workerDal)
        {
            _workerDal = workerDal;
        }

        [CacheRemoveAspect("IWorkerService.Get")]
        [SecuredOperation("worker.admin,admin")]
        public IResult Delete(Worker worker) 
        {
            _workerDal.Delete(worker);
            return new SuccessResult(Messages.WorkerDeleted);
        }

        [CacheAspect]
        public IDataResult<List<Worker>> GetAll()
        {
            return new SuccessDataResult<List<Worker>>(_workerDal.GetAll(),Messages.WorkersListed);
        }

        public IDataResult<Worker> GetById(int id)
        {
            return new SuccessDataResult<Worker>(_workerDal.Get(w=>w.WorkerId==id));
        }

        [SecuredOperation("worker.admin,admin")]
        [ValidationAspect(typeof(WorkerValidator))]
        [CacheRemoveAspect("IWorkerService.Get")]
        public IResult Add(Worker worker)
        {
            if (worker.FirstName.Length < 2)
            {
                return new ErrorResult();
            }
            else {
                _workerDal.Add(worker);
                return new SuccessResult(Messages.WorkerAdded);
            };
        }

        [CacheRemoveAspect("IWorkerService.Get")]
        [ValidationAspect(typeof(WorkerValidator))]
        [SecuredOperation("worker.admin,admin")]
        public IResult Update(Worker worker)
        {
            _workerDal.Update(worker);
            return new SuccessResult(Messages.WorkerUpdated);
        }
    }
}
