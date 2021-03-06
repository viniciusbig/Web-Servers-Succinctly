﻿/*
Copyright (c) 2015, Marc Clifton
All rights reserved.

Redistribution and use in source and binary forms, with or without modification,
are permitted provided that the following conditions are met:

* Redistributions of source code must retain the above copyright notice, this list
  of conditions and the following disclaimer. 

* Redistributions in binary form must reproduce the above copyright notice, this 
  list of conditions and the following disclaimer in the documentation and/or other
  materials provided with the distribution. 
 
* Neither the name of MyXaml nor the names of its contributors may be
  used to endorse or promote products derived from this software without specific
  prior written permission. 

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Clifton.WebServer
{
	/// <summary>
	/// Route requests to an application-defined handler.
	/// </summary>
	public class RouteHandler
	{
		protected RouteTable routeTable;
		protected SessionManager sessionManager;

		public RouteHandler(RouteTable routeTable, SessionManager sessionManager)
		{
			this.routeTable = routeTable;
			this.sessionManager = sessionManager;
		}

		/// <summary>
		/// Route the request.  If no route exists, the workflow continues, otherwise, we return the route handler's continuation state.
		/// </summary>
		public WorkflowState Route(WorkflowContinuation<HttpListenerContext> workflowContinuation, HttpListenerContext context)
		{
			WorkflowState ret = WorkflowState.Continue;
			RouteEntry entry = null;
			Session session = sessionManager != null ? sessionManager[context] : null;

			if (routeTable.TryGetRouteEntry(context.Verb(), context.Path(), out entry))
			{
				if (entry.RouteProvider != null)
				{
					ret = entry.RouteProvider(workflowContinuation, context, session);
				}
			}

			return ret;
		}
	}
}
