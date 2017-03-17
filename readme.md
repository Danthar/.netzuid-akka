# DotNet Zuid samples

This repository contains the code demo's i have used in my .NetZuid talk.

For the Clustering sample i have used an instance of the **[Lighthouse](https://github.com/petabridge/lighthouse)** dedicated Seed node. If you want to run the Clustering sample. You will need to clone and build that repository.
The configuration for the lighthouse Seed node needs to be changed in order for it to run.

- Make sure the same akka.net dependencies are used (currently the sample uses version 1.1.3)
- Make sure Hyperion is used (currently using 1.1.3.32-beta)
- Make sure the same ActorSystem name is used: "ClusterSystem"

The HOCON config then would look like this:

 			lighthouse{
				actorsystem: "ClusterSystem" 
			}
			akka {
				actor { 
					provider = "Akka.Cluster.ClusterActorRefProvider, Akka.Cluster"
						serializers 
					{
						hyperion = "Akka.Serialization.HyperionSerializer, Akka.Serialization.Hyperion"
					}
					serialization-bindings {
						"System.Object" = hyperion
					}
				}
			remote {
				log-remote-lifecycle-events = DEBUG
				helios.tcp {
					transport-class = "Akka.Remote.Transport.Helios.HeliosTcpTransport, Akka.Remote"
					applied-adapters = []
					transport-protocol = tcp
					#will be populated with a dynamic host-name at runtime if left uncommented
					#public-hostname = "POPULATE STATIC IP HERE"
					hostname = "127.0.0.1"
					port = 4053
				}
			}            
			loggers = ["Akka.Logger.NLog.NLogLogger,Akka.Logger.NLog"]
			cluster {
				#will inject this node as a self-seed node at run-time
				seed-nodes = [] #manually populate other seed nodes here, i.e. "akka.tcp://lighthouse@127.0.0.1:4053", "akka.tcp://lighthouse@127.0.0.1:4044"
				roles = [lighthouse]
			}
		}



If you wish to learn more about Akka.NET check out our website at http://getakka.net, go to the github repository https://github.com/akkadotnet/ or follow the self-paced bootcamp by Petabridge on https://petabridge.com/bootcamp/