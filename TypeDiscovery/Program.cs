// Copyright (c) 2009 rubicon informationstechnologie gmbh
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using Remotion.Configuration.TypeDiscovery;
using Remotion.Logging;
using Remotion.Reflection.TypeDiscovery;
using System.Linq;
using Remotion.Text;

namespace TypeDiscovery
{
  public class Program
  {
    private static void Main ()
    {
      LogManager.Initialize ();

      // ConfigureTypeDiscovery ();
      // ConfigureTypeDiscoveryService ();

      var typeDiscoveryService = ContextAwareTypeDiscoveryUtility.GetTypeDiscoveryService ();
      Console.WriteLine ("Type discovery service type: {0}", typeDiscoveryService.GetType ());
      Console.WriteLine ();

      var types = typeDiscoveryService.GetTypes (null, false);
      var assemblies = types.Cast<Type> ().Select (t => t.Assembly).Distinct ().ToArray ();
      Console.WriteLine ("Found {0} types from {1} assemblies", types.Count, assemblies.Length);
      Console.WriteLine ();

      Console.WriteLine ("Assemblies:");
      Console.WriteLine (SeparatedStringBuilder.Build (", ", assemblies, a => a.GetName ().Name));
      Console.WriteLine ();

      Console.WriteLine ("Types (first 100): ");
      Console.WriteLine (SeparatedStringBuilder.Build (", ", types.Cast<Type> ().Take (100), t => t.Name));
    }

    private static void ConfigureTypeDiscovery()
    {
      var configuration = new TypeDiscoveryConfiguration ();
      configuration.Mode = TypeDiscoveryMode.SpecificRootAssemblies;
      
      var nameSpecification = new ByNameRootAssemblyElement { Name = "Library1", IncludeReferencedAssemblies = true };
      configuration.SpecificRootAssemblies.ByName.Add (nameSpecification);

      var includePattern = new ByFileIncludeRootAssemblyElement { FilePattern = "Library*.dll" };
      configuration.SpecificRootAssemblies.ByFile.Add (includePattern);

      var excludePattern = new ByFileExcludeRootAssemblyElement { FilePattern = "Library2.dll" };
      configuration.SpecificRootAssemblies.ByFile.Add (excludePattern);

      TypeDiscoveryConfiguration.SetCurrent (configuration);
    }

    private static void ConfigureTypeDiscoveryService ()
    {
      ContextAwareTypeDiscoveryUtility.DefaultNonDesignModeService = new CustomTypeDiscoveryService ();
    }
  }
}