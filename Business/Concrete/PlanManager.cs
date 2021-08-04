using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Concrete
{
    public class PlanManager : IPlanService
    {
        IPlanDal _planDal;

        public PlanManager(IPlanDal planDal)
        {
            _planDal = planDal;
        }

        [ValidationAspect(typeof(PlanValidator))]
        [CacheRemoveAspect("IPlanService.Get")]
        //[SecuredOperation("plan.admin,admin")]
        public IResult Add(Plan plan)
        {
            _planDal.Add(plan);
            return new SuccessResult(Messages.PlanAdded);
        }

        [CacheRemoveAspect("IPlanService.Get")]
        [SecuredOperation("plan.admin,admin")]
        public IResult Delete(Plan plan)
        {
            _planDal.Delete(plan);
            return new SuccessResult(Messages.PlanDeleted);
        }

        [CacheAspect]
        public IDataResult<List<Plan>> GetAll()
        {
            return new SuccessDataResult<List<Plan>>(_planDal.GetAll(),Messages.PlansListed);
        }

        public IDataResult<Plan> GetById(int planId)
        {
            return new SuccessDataResult<Plan>(_planDal.Get(p=> p.PlanId==planId));
            
        }

        public IDataResult<List<PlanDetailDto>> GetPlanDetail()
        {
            return new SuccessDataResult<List<PlanDetailDto>>(_planDal.GetPlanDetails(),Messages.PlanDetailListed);
        }

        public IDataResult<PlanDetailDto> GetPlanDetailById(int planId)
        {
            return new SuccessDataResult<PlanDetailDto>(_planDal.GetPlanDetail(planId));
        }

        [CacheRemoveAspect("IPlanService.Get")]
        [ValidationAspect(typeof(PlanValidator))]
        [SecuredOperation("plan.admin,admin")]
        public IResult Update(Plan plan)
        {
            _planDal.Update(plan);
            return new SuccessResult(Messages.PlanUpdated);
        }
    }
}
