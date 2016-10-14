using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using Microsoft.Practices.Unity;

namespace SmartHouse.WebApiMono
{
	public class ScopeContainer : IDependencyScope
	{
		protected IUnityContainer container;

		public ScopeContainer(IUnityContainer container)
		{
			if (container == null)
			{
				throw new ArgumentNullException("container");
			}
			this.container = container;
		}

		public object GetService(Type serviceType)
		{
			if (container.IsRegistered(serviceType))
			{
				return container.Resolve(serviceType);
			}
			else
			{
				return null;
			}
		}

		public IEnumerable<object> GetServices(Type serviceType)
		{
			if (container.IsRegistered(serviceType))
			{
				return container.ResolveAll(serviceType);
			}
			else
			{
				return new List<object>();
			}
		}

		public void Dispose()
		{
			container.Dispose();
		}
	}

	class IoCContainer : ScopeContainer, IDependencyResolver
	{
		public IoCContainer(IUnityContainer container)
			: base(container)
		{
		}

		public IDependencyScope BeginScope()
		{
			var child = container.CreateChildContainer();
			return new ScopeContainer(child);
		}
	}

}
public class UnityResolver : IDependencyResolver
{
	protected IUnityContainer container;

	public UnityResolver(IUnityContainer container)
	{
		if (container == null)
		{
			throw new ArgumentNullException("container");
		}
		this.container = container;
	}

	public object GetService(Type serviceType)
	{
		try
		{
			return container.Resolve(serviceType);
		}
		catch (ResolutionFailedException)
		{
			return null;
		}
	}

	public IEnumerable<object> GetServices(Type serviceType)
	{
		try
		{
			return container.ResolveAll(serviceType);
		}
		catch (ResolutionFailedException)
		{
			return new List<object>();
		}
	}

	public IDependencyScope BeginScope()
	{
		var child = container.CreateChildContainer();
		return new UnityResolver(child);
	}

	public void Dispose()
	{
		container.Dispose();
	}
}