using TodoList.Core.Interfaces;
using TodoList.DAL;
using Unity;
using TodoList.Core.Framework.Cache;

namespace TodoList.Service.IOC
{
    public sealed class ContainerFactory
    {
        public static IUnityContainer CreateUnityContainer(IUnityContainer container)
        {
            container.RegisterType<IMemoryCacheEx, MemoryCacheEx>();
            container.RegisterType<ITodoListService, TodoListService>();
            container.RegisterType<ITodoListDAL, TodoListDAL>();
  
            return container;
        }
    }
}
