using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core;
using TaskManager.Core.Entities.Projects;
using TaskManager.Core.Services.Contract;

namespace TaskManager.Service.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProjectService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Project> CreateAsync(Project project)
        {
            // Calculate TotalPrice before saving
            project.TotalPrice = project.DesignCost + project.ProductionCost;

            await _unitOfWork.Repository<Project>().AddAsync(project);
            await _unitOfWork.CompleteAsync();
            return project;
        }

        public async Task<Project?> UpdateCostsAsync(int id, decimal designCost
                                                     , decimal productionCost)
        {
            var project = await _unitOfWork.Repository<Project>().GetByIdAsync(id);

            if (project is null) return null;

            project.DesignCost = designCost;
            project.ProductionCost = productionCost;

            // Recalculate TotalPrice whenever costs change
            project.TotalPrice = designCost + productionCost;

            _unitOfWork.Repository<Project>().Update(project);
            await _unitOfWork.CompleteAsync();
            return project;
        }

        public async Task<Project?> ChangeStatusToSentAsync(int id)
        {
            var project = await _unitOfWork.Repository<Project>().GetByIdAsync(id);

            if (project is null) return null;

            if (project.Status == ProjectStatus.Sent)
                return project;

            project.Status = ProjectStatus.Sent;

            _unitOfWork.Repository<Project>().Update(project);
            await _unitOfWork.CompleteAsync();
            return project;
        }
    }
}
