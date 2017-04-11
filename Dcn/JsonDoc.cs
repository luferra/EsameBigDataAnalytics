using System;
using System.Collections.Generic;
using System.Text;
//using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Dcn
{
	public class JsonDoc
	{
		public Dictionary<string, Source> sources = new Dictionary<string, Source>();
        public Dictionary<string, Destination> destinations = new Dictionary<string, Destination>();
        public Dictionary<string, Filter> filters = new Dictionary<string, Filter>();
        public Dictionary<string, Aggregate> aggregates = new Dictionary<string, Aggregate>();
		public Dictionary<string, Join> joins = new Dictionary<string, Join>();
        public Dictionary<string, TriggerAction> triggerActions = new Dictionary<string, TriggerAction>();
        public Dictionary<string, TriggerEvent> triggerEvents = new Dictionary<string, TriggerEvent>();
        public List<Edge> edges = new List<Edge>();

		public string json = "";
		public List<string> document = new List<string>();

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="formMain">Reference to the main form</param>
		public JsonDoc(string json)
		{
			this.json = json;
			Console.WriteLine("");
			Console.WriteLine(json);
			Console.WriteLine("");
		}

		/// <summary>
		/// Parse the json file
		/// </summary>
		/// <param name="jsonString">Json string</param>
		/// <returns>True if the syntax is correct</returns>
		public bool ParseJson()
		{
			try
			{
				document = new List<string>();

				string jsonString = json;

				JObject root = JObject.Parse(jsonString);

				JArray items = (JArray)root["elements"]["nodes"];

				JObject item;
				JToken jtoken;

				JObject conditionsItem;
				JToken conditionsJtoken;

				int cont = 0;

				for (int i = 0; i < items.Count; i++)
				{
					item = (JObject)items[i];
					jtoken = item.First;

					while (jtoken != null)
					{

						switch (((JProperty)jtoken).Value["object"].ToString())
						{

						case "source":

							Source source = new Source();
							source._id = ((JProperty)jtoken).Value["id"].ToString();
							source._name = ((JProperty)jtoken).Value["name"].ToString();
							source._object = ((JProperty)jtoken).Value["object"].ToString();
							source._table = ((JProperty)jtoken).Value["table"].ToString();

							Console.WriteLine("Source");
							Console.WriteLine("\tId: " + source._id);
							Console.WriteLine("\tName: " + source._name);

							JArray jArrayConditions = (JArray)((JProperty)jtoken).Value["conditions"];

							if (jArrayConditions.Count > 0)
							{
								conditionsItem = (JObject)jArrayConditions[0];
								conditionsJtoken = jArrayConditions.First;

								while (conditionsJtoken != null)
								{

									Source.Condition sourceCondition = new Source.Condition();
									sourceCondition._attribute = conditionsJtoken["attribute"].ToString();
									sourceCondition._operator = conditionsJtoken["operator"].ToString();
									sourceCondition._value = conditionsJtoken["value"].ToString();

									source._conditions.Add(sourceCondition);

									Console.WriteLine("\tCondition");
									Console.WriteLine("\t\tAttribute: " + sourceCondition._attribute);
									Console.WriteLine("\t\tOperator: " + sourceCondition._operator);
									Console.WriteLine("\t\tValue: " + sourceCondition._value);

									conditionsJtoken = conditionsJtoken.Next;
								}
							}

							sources.Add(source._id, source);
							break;

						case "destination":

							Destination destination = new Destination();
							destination._id = ((JProperty)jtoken).Value["id"].ToString();
							destination._name = ((JProperty)jtoken).Value["name"].ToString();
							destination._object = ((JProperty)jtoken).Value["object"].ToString();

							Console.WriteLine("Destination");
							Console.WriteLine("\tId: " + destination._id);
							Console.WriteLine("\tName: " + destination._name);

							jArrayConditions = (JArray)((JProperty)jtoken).Value["conditions"];

							if (jArrayConditions.Count > 0)
							{
								conditionsItem = (JObject)jArrayConditions[0];
								conditionsJtoken = jArrayConditions.First;

								while (conditionsJtoken != null)
								{

									Destination.Condition destinationCondition = new Destination.Condition();
									destinationCondition._attribute = conditionsJtoken["attribute"].ToString();
									destinationCondition._operator = conditionsJtoken["operator"].ToString();
									destinationCondition._value = conditionsJtoken["value"].ToString();

									destination._conditions.Add(destinationCondition);

									Console.WriteLine("\tCondition");
									Console.WriteLine("\t\tAttribute: " + destinationCondition._attribute);
									Console.WriteLine("\t\tOperator: " + destinationCondition._operator);
									Console.WriteLine("\t\tValue: " + destinationCondition._value);

									conditionsJtoken = conditionsJtoken.Next;
								}
							}

							destinations.Add(destination._id, destination);
							break;

						case "filter":

							Filter filter = new Filter();
							filter._id = ((JProperty)jtoken).Value["id"].ToString();
							filter._name = ((JProperty)jtoken).Value["name"].ToString();
							filter._object = ((JProperty)jtoken).Value["object"].ToString();

							Console.WriteLine("Filter");
							Console.WriteLine("\tId: " + filter._id);
							Console.WriteLine("\tName: " + filter._name);

							jArrayConditions = (JArray)((JProperty)jtoken).Value["conditions"];

							if (jArrayConditions.Count > 0)
							{
								conditionsItem = (JObject)jArrayConditions[0];
								conditionsJtoken = jArrayConditions.First;

								while (conditionsJtoken != null)
								{

									try
									{

										Filter.Condition filterCondition = new Filter.Condition();
										filterCondition._attribute = conditionsJtoken["attribute"].ToString();
										filterCondition._operator = conditionsJtoken["operator"].ToString();

										if ((filterCondition._operator == "RANGE") ||
											(filterCondition._operator == "NOT RANGE"))
										{
											filterCondition._value1 = conditionsJtoken["value1"].ToString();
											filterCondition._value2 = conditionsJtoken["value2"].ToString();
										}
										else
										{
											filterCondition._value = conditionsJtoken["value"].ToString();
										}

										filter._conditions.Add(filterCondition);

										Console.WriteLine("\tCondition");
										Console.WriteLine("\t\tAttribute: " + filterCondition._attribute);
										Console.WriteLine("\t\tOperator: " + filterCondition._operator);
										Console.WriteLine("\t\tValue: " + filterCondition._value);
									}
									catch (Exception es)
									{
									}
									conditionsJtoken = conditionsJtoken.Next;
								}
							}

							jArrayConditions = (JArray)((JProperty)jtoken).Value["orconditions"];

							if (jArrayConditions.Count > 0)
							{
								conditionsItem = (JObject)jArrayConditions[0];
								conditionsJtoken = jArrayConditions.First;

								while (conditionsJtoken != null)
								{

									try
									{

										Filter.Condition filterCondition = new Filter.Condition();
										filterCondition._attribute = conditionsJtoken["attribute"].ToString();
										filterCondition._operator = conditionsJtoken["operator"].ToString();

										if ((filterCondition._operator == "RANGE") ||
											(filterCondition._operator == "NOT RANGE"))
										{
											filterCondition._value1 = conditionsJtoken["value1"].ToString();
											filterCondition._value2 = conditionsJtoken["value2"].ToString();
										}
										else
										{
											filterCondition._value = conditionsJtoken["value"].ToString();
										}

										filter._orconditions.Add(filterCondition);

										Console.WriteLine("\tOrcondition");
										Console.WriteLine("\t\tAttribute: " + filterCondition._attribute);
										Console.WriteLine("\t\tOperator: " + filterCondition._operator);
										Console.WriteLine("\t\tValue: " + filterCondition._value);
									}
									catch (Exception es)
									{
									}
									conditionsJtoken = conditionsJtoken.Next;
								}
							}

							filters.Add(filter._id, filter);
							break;

						case "aggregate":

							try
							{

								Aggregate aggregate = new Aggregate();
								aggregate._id = ((JProperty)jtoken).Value["id"].ToString();
								aggregate._name = ((JProperty)jtoken).Value["name"].ToString();
								aggregate._object = ((JProperty)jtoken).Value["object"].ToString();
									aggregate._nds = ((JProperty)jtoken).Value["nds"].ToString();

								Console.WriteLine("Aggregate");
								Console.WriteLine("\tId: " + aggregate._id);
								Console.WriteLine("\tName: " + aggregate._name);
									Console.WriteLine("\tnds: " + aggregate._nds);

								JToken jTokenAggregate = (JToken)((JProperty)jtoken).Value["settings"];
									Console.WriteLine("\tSettings: ");

                                aggregate._settings._aggregate_data_name = jTokenAggregate["aggregate_data_name"].ToString();
                                Console.WriteLine("\t\tAggregate_Data_Name: " + aggregate._settings._aggregate_data_name);
								aggregate._settings._time_interval = jTokenAggregate["time_interval"].ToString();
								Console.WriteLine("\t\tTime_Interval: " + aggregate._settings._time_interval);
								aggregate._settings._time_unit = jTokenAggregate["time_unit"].ToString();
								Console.WriteLine("\t\tTime_Unit: " + aggregate._settings._time_unit);

									JArray jArraySchema = (JArray)((JProperty)jtoken).Value["nds"];

									if (jArraySchema.Count > 0)
									{
										conditionsItem = (JObject)jArraySchema[0];
										conditionsJtoken = jArraySchema.First;

										while (conditionsJtoken != null)
										{
											try{
												

											Aggregate.Condition aggCondition = new Aggregate.Condition();
											aggCondition._typec = conditionsJtoken["type"].ToString();
											aggCondition._namec = conditionsJtoken["name"].ToString();
											//aggCondition._unitc = conditionsJtoken["unit"].ToString();
											aggCondition._selectedc = conditionsJtoken["selected"].ToString();
											aggCondition._enabledc = conditionsJtoken["enabled"].ToString();

											aggregate._conditions.Add(aggCondition);

											Console.WriteLine("\tCondition");
											Console.WriteLine("\t\tType: " + aggCondition._typec);
											Console.WriteLine("\t\tName: " + aggCondition._namec);
											//Console.WriteLine("\t\tUnit: " + aggCondition._unitc);
											Console.WriteLine("\t\tSelected: " + aggCondition._selectedc);
											Console.WriteLine("\t\tEnabled: " + aggCondition._enabledc);
												}
											catch (Exception es)
											{
											}
											conditionsJtoken = conditionsJtoken.Next;
										}
									}


								aggregates.Add(aggregate._id, aggregate);
							}
							catch (Exception es)
							{

								Console.WriteLine(es.ToString());
							}
							break;

						case "triggerEvent":

							TriggerEvent triggerEvent = new TriggerEvent();
							triggerEvent._id = ((JProperty)jtoken).Value["id"].ToString();
							triggerEvent._name = ((JProperty)jtoken).Value["name"].ToString();
							triggerEvent._object = ((JProperty)jtoken).Value["object"].ToString();

							Console.WriteLine("TriggerEvent");
							Console.WriteLine("\tId: " + triggerEvent._id);
							Console.WriteLine("\tName: " + triggerEvent._name);

							JToken jTokenTrigger = (JToken)((JProperty)jtoken).Value["triggerOn"];
							Console.WriteLine("\tTriggerOn: ");

							triggerEvent._triggerOn._trigger_interval = jTokenTrigger["trigger_interval"].ToString();
							Console.WriteLine("\tTrigger_Interval: " + triggerEvent._triggerOn._trigger_interval);

							JToken jTokenTriggerContidion = (JToken)((JProperty)jtoken).Value["triggerOn"]["trigger_condition"];
							Console.WriteLine("\tTrigger_Condition");

							triggerEvent._triggerOn._triggerCondition._expression = jTokenTriggerContidion["expression"].ToString();
							Console.WriteLine("\t\tExpression: " + triggerEvent._triggerOn._triggerCondition._expression);

							triggerEvent._triggerOn._triggerCondition._data_value = jTokenTriggerContidion["data_value"].ToString();
							Console.WriteLine("\t\tData_Value: " + triggerEvent._triggerOn._triggerCondition._data_value);

							jArrayConditions = (JArray)((JProperty)jtoken).Value["triggerOn"]["conditions"];
							Console.WriteLine("\t\tConditions");

							if ((jArrayConditions != null) && (jArrayConditions.Count > 0))
							{
								conditionsItem = (JObject)jArrayConditions[0];
								conditionsJtoken = jArrayConditions.First;

								while (conditionsJtoken != null)
								{

									TriggerEvent.Condition triggerEventCondition = new TriggerEvent.Condition();
									triggerEventCondition._attribute = conditionsJtoken["attribute"].ToString();
									triggerEventCondition._operator = conditionsJtoken["operator"].ToString();

									if ((triggerEventCondition._operator == "RANGE") ||
										(triggerEventCondition._operator == "NOT RANGE"))
									{
										triggerEventCondition._value1 = conditionsJtoken["value1"].ToString();
										triggerEventCondition._value2 = conditionsJtoken["value2"].ToString();
									}
									else
									{
										triggerEventCondition._value = conditionsJtoken["value"].ToString();
									}

									Console.WriteLine("\t\t\tAttribute: " + triggerEventCondition._attribute);
									Console.WriteLine("\t\t\tOperator: " + triggerEventCondition._operator);
									Console.WriteLine("\t\t\tValue: " + triggerEventCondition._value);

									triggerEvent._triggerOn._conditions.Add(triggerEventCondition);
									conditionsJtoken = conditionsJtoken.Next;
								}
							}

							jArrayConditions = (JArray)((JProperty)jtoken).Value["triggerOn"]["orconditions"];
							Console.WriteLine("\t\tOrConditions");

							if ((jArrayConditions != null) && (jArrayConditions.Count > 0))
							{
								conditionsItem = (JObject)jArrayConditions[0];
								conditionsJtoken = jArrayConditions.First;

								while (conditionsJtoken != null)
								{

									TriggerEvent.Condition triggerEventCondition = new TriggerEvent.Condition();
									triggerEventCondition._attribute = conditionsJtoken["attribute"].ToString();
									triggerEventCondition._operator = conditionsJtoken["operator"].ToString();

									if ((triggerEventCondition._operator == "RANGE") ||
										(triggerEventCondition._operator == "NOT RANGE"))
									{
										triggerEventCondition._value1 = conditionsJtoken["value1"].ToString();
										triggerEventCondition._value2 = conditionsJtoken["value2"].ToString();
									}
									else
									{
										triggerEventCondition._value = conditionsJtoken["value"].ToString();
									}

									Console.WriteLine("\t\t\tAttribute: " + triggerEventCondition._attribute);
									Console.WriteLine("\t\t\tOperator: " + triggerEventCondition._operator);
									Console.WriteLine("\t\t\tValue: " + triggerEventCondition._value);

									triggerEvent._triggerOn._orconditions.Add(triggerEventCondition);
									conditionsJtoken = conditionsJtoken.Next;
								}
							}

							jTokenTrigger = (JToken)((JProperty)jtoken).Value["triggerOff"];
							Console.WriteLine("\tTriggerOff: ");

							triggerEvent._triggerOff._trigger_interval = jTokenTrigger["trigger_interval"].ToString();
							Console.WriteLine("\tTrigger_Interval: " + triggerEvent._triggerOff._trigger_interval);

							jTokenTriggerContidion = (JToken)((JProperty)jtoken).Value["triggerOff"]["trigger_condition"];
							Console.WriteLine("\tTrigger_Condition");

							triggerEvent._triggerOff._triggerCondition._expression = jTokenTriggerContidion["expression"].ToString();
							Console.WriteLine("\t\tExpression: " + triggerEvent._triggerOff._triggerCondition._expression);
							triggerEvent._triggerOff._triggerCondition._data_value = jTokenTriggerContidion["data_value"].ToString();
							Console.WriteLine("\t\tData_Value: " + triggerEvent._triggerOff._triggerCondition._data_value);

							jArrayConditions = (JArray)((JProperty)jtoken).Value["triggerOff"]["conditions"];
							Console.WriteLine("\t\tConditions");

							if ((jArrayConditions != null) && (jArrayConditions.Count > 0))
							{
								conditionsItem = (JObject)jArrayConditions[0];
								conditionsJtoken = jArrayConditions.First;

								while (conditionsJtoken != null)
								{

									TriggerEvent.Condition triggerEventCondition = new TriggerEvent.Condition();
									triggerEventCondition._attribute = conditionsJtoken["attribute"].ToString();
									triggerEventCondition._operator = conditionsJtoken["operator"].ToString();


									if ((triggerEventCondition._operator == "RANGE") ||
										(triggerEventCondition._operator == "NOT RANGE"))
									{
										triggerEventCondition._value1 = conditionsJtoken["value1"].ToString();
										triggerEventCondition._value2 = conditionsJtoken["value2"].ToString();
									}
									else
									{
										triggerEventCondition._value = conditionsJtoken["value"].ToString();
									}

									Console.WriteLine("\t\t\tAttribute: " + triggerEventCondition._attribute);
									Console.WriteLine("\t\t\tOperator: " + triggerEventCondition._operator);
									Console.WriteLine("\t\t\tValue: " + triggerEventCondition._value);

									triggerEvent._triggerOff._conditions.Add(triggerEventCondition);
									conditionsJtoken = conditionsJtoken.Next;
								}
							}

							jArrayConditions = (JArray)((JProperty)jtoken).Value["triggerOff"]["orconditions"];
							Console.WriteLine("\t\tOrConditions");

							if ((jArrayConditions != null) && (jArrayConditions.Count > 0))
							{
								conditionsItem = (JObject)jArrayConditions[0];
								conditionsJtoken = jArrayConditions.First;

								while (conditionsJtoken != null)
								{

									TriggerEvent.Condition triggerEventCondition = new TriggerEvent.Condition();
									triggerEventCondition._attribute = conditionsJtoken["attribute"].ToString();
									triggerEventCondition._operator = conditionsJtoken["operator"].ToString();


									if ((triggerEventCondition._operator == "RANGE") ||
										(triggerEventCondition._operator == "NOT RANGE"))
									{
										triggerEventCondition._value1 = conditionsJtoken["value1"].ToString();
										triggerEventCondition._value2 = conditionsJtoken["value2"].ToString();
									}
									else
									{
										triggerEventCondition._value = conditionsJtoken["value"].ToString();
									}

									Console.WriteLine("\t\t\tAttribute: " + triggerEventCondition._attribute);
									Console.WriteLine("\t\t\tOperator: " + triggerEventCondition._operator);
									Console.WriteLine("\t\t\tValue: " + triggerEventCondition._value);

									triggerEvent._triggerOff._orconditions.Add(triggerEventCondition);
									conditionsJtoken = conditionsJtoken.Next;
								}
							}

							triggerEvents.Add(triggerEvent._id, triggerEvent);
							break;

						case "triggerAction":

							try
							{

								TriggerAction triggerAction = new TriggerAction();
								triggerAction._id = ((JProperty)jtoken).Value["id"].ToString();
								triggerAction._name = ((JProperty)jtoken).Value["name"].ToString();
								triggerAction._object = ((JProperty)jtoken).Value["object"].ToString();
								triggerAction._event = ((JProperty)jtoken).Value["event"].ToString();
								triggerAction._event_state = ((JProperty)jtoken).Value["event_state"].ToString();

								Console.WriteLine("TriggerAction");
								Console.WriteLine("\tId: " + triggerAction._id);
								Console.WriteLine("\tName: " + triggerAction._name);
								Console.WriteLine("\tEvent: " + triggerAction._event);
								Console.WriteLine("\tEventState: " + triggerAction._event_state);

								triggerActions.Add(triggerAction._id, triggerAction);
							}
							catch (Exception es)
							{

								Console.WriteLine(es.ToString());
							}
							break;
								
						case "join":

								try
								{

									Join join = new Join();
									join._id = ((JProperty)jtoken).Value["id"].ToString();
									join._name = ((JProperty)jtoken).Value["name"].ToString();
									join._object = ((JProperty)jtoken).Value["object"].ToString();
									join._join_type = ((JProperty)jtoken).Value["join_type"].ToString();
									switch (join._join_type)
									{
										case "Inner":
											join._join_type = "inner";
											break;
										case "Outer":
											join._join_type = "outer";
											break;
										case "Left outer":
											join._join_type = "left_outer";
											break;	
										case "Right outer":
											join._join_type = "right_outer";
											break;	
										default:
											break;
									}

									Console.WriteLine("Join");
									Console.WriteLine("\tId: " + join._id);
									Console.WriteLine("\tName: " + join._name);
									Console.WriteLine("\tObject: " + join._object);
									Console.WriteLine("\tJoin_type: " + join._join_type );

									jArrayConditions = (JArray)((JProperty)jtoken).Value["join_schema"];

									if (jArrayConditions.Count > 0)
									{
										conditionsItem = (JObject)jArrayConditions[0];
										conditionsJtoken = jArrayConditions.First;

										while (conditionsJtoken != null)
										{

											Join.Condition joinSchema = new Join.Condition();
											joinSchema._property = conditionsJtoken["property"].ToString();
											joinSchema._property2 = conditionsJtoken["property2"].ToString();
											joinSchema._ids = conditionsJtoken["ids"].ToString();

											joinSchema._ids = joinSchema._ids.Substring(1, joinSchema._ids.Length-1);
											joinSchema._id0 = joinSchema._ids.Split(',');
											joinSchema._id00 = joinSchema._id0[0].Substring(4, joinSchema._id0[0].Length - 5);
											joinSchema._id11 = joinSchema._id0[1].Substring(4, joinSchema._id0[0].Length - 5);

											join._join_schema.Add(joinSchema);

											Console.WriteLine("\tjoinSchema");
											Console.WriteLine("\t\tProperty: " + joinSchema._property);
											Console.WriteLine("\t\tProperty2: " + joinSchema._property2);
											Console.WriteLine("\t\tid0: " + joinSchema._id00);
											Console.WriteLine("\t\tid1: " + joinSchema._id11);


											conditionsJtoken = conditionsJtoken.Next;
										}
									}

									joins.Add(join._id, join);
								}
								catch (Exception es)
								{
									Console.WriteLine(es.Message);
								}
								break;

						default:
							break;
						}

						jtoken = jtoken.Next;
					}
				}

				// Discover edges
				items = (JArray)root["elements"]["edges"];

				// Foreach egde
				for (int i = 0; i < items.Count; i++)
				{
					item = (JObject)items[i];
					jtoken = item.First;

					try
					{

						while (jtoken != null)
						{

							Edge edge = new Edge();
							edge._id = ((JProperty)jtoken).Value["id"].ToString();
							edge._source = ((JProperty)jtoken).Value["source"].ToString();
							edge._target = ((JProperty)jtoken).Value["target"].ToString();

							Console.WriteLine("Edge");
							Console.WriteLine("\tId: " + edge._id);
							Console.WriteLine("\tSource: " + edge._source);
							Console.WriteLine("\tTarget: " + edge._target);

							edges.Add(edge);

							jtoken = jtoken.Next;
						}
					}
					catch (Exception es)
					{
						Console.WriteLine(es.Message);
					}
				}

				return true;
			}
			catch (Exception es)
			{

				// Error message
				Console.WriteLine("Error in json format: " + es.Message);
				return false;
			}
		}

		/// <summary>
		/// Create the DSN document
		/// </summary>
		/// <param name="outputString">The text document</param>
		/// <returns></returns>
		public bool CreateDSN()
		{

			try
			{

				////***************************************************************************
				//// Comments
				////***************************************************************************
				document.Add("// Created by StreamLoader on " + DateTime.Now.ToUniversalTime() + ";");

				// Init state
				document.Add("package spark_translate;");
				document.Add("import org.apache.spark.sql.SparkSession;");
				document.Add("import org.apache.spark.sql.Column;");
				document.Add("import org.apache.spark.sql.Dataset;");
				document.Add("import org.apache.spark.sql.Row;");
				document.Add("import static org.apache.spark.sql.functions.col;");
				document.Add("import org.apache.spark.sql.functions;");
				document.Add("import org.apache.spark.sql.functions.*;");
				document.Add("import org.apache.spark.sql.types.StructType;");
				document.Add("import org.apache.spark.sql.catalyst.expressions.aggregate.Average;");
				document.Add("import org.apache.spark.sql.catalyst.expressions.aggregate.First;");

				document.Add("\n");
				document.Add("public class spark_script {");
				document.Add("\tpublic static void main(String[] args) throws Exception {");
				document.Add("\tSparkSession spark = SparkSession");
				document.Add("\t.builder()");
				document.Add("\t.appName(\"Java Spark Translator\")");
				document.Add("\t.master(\"local\")");
				document.Add("\t.getOrCreate();");
				document.Add("");


				//***************************************************************************
				// Sources
				//***************************************************************************
				foreach (KeyValuePair<string, Source> source in sources)
				{
					StringBuilder stringBuilder = new StringBuilder();

					//// Create the destination
					//stringBuilder.Append("\t@" + source.Value._name + ": discovery(");

					if ((source.Value._triggerAction != "undef"))
					{
						for (int i = 0; i < source.Value._conditions.Count - 1; i++)
						{
							Source.Condition condition = source.Value._conditions[i];
							if (source.Value._conditions[1]._value == "rain")
							{
								stringBuilder.Append("\tStructType rainSchema = new StructType().add(\"id\",\"integer\").add(\"altitude\",\"double\").add(\"city_name\",\"string\").add(\"latitude\",\"double\").add(\"longitude\",\"double\").add(\"rainfall\",\"double\").add(\"station_name\",\"string\").add(\"time\",\"timestamp\");\n");
							}
							else if (source.Value._conditions[1]._value == "humidity")
							{
								stringBuilder.Append("\tStructType humiditySchema = new StructType().add(\"id\",\"integer\").add(\"altitude\",\"double\").add(\"city_name\",\"string\").add(\"latitude\",\"double\").add(\"longitude\",\"double\").add(\"humidity\",\"double\").add(\"station_name\",\"string\").add(\"time\",\"timestamp\");\n");
							}
							else if (source.Value._conditions[1]._value == "temperature")
							{
								stringBuilder.Append("\tStructType temperatureSchema = new StructType().add(\"id\",\"integer\").add(\"altitude\",\"double\").add(\"city_name\",\"string\").add(\"latitude\",\"double\").add(\"longitude\",\"double\").add(\"temperature\",\"double\").add(\"station_name\",\"string\").add(\"time\",\"timestamp\");\n");
							}
							else if (source.Value._conditions[1]._value == "twitter")
							{
								stringBuilder.Append("\tStructType twitterSchema = new StructType().add(\"id_str\",\"integer\").add(\"latitude\",\"double\").add(\"longitude\",\"double\").add(\"tweet\",\"string\").add(\"time\",\"timestamp\");\n");
							}



							stringBuilder.Append("\tDataset<Row> " + source.Value._id +
												 " = spark.read().schema(" + source.Value._conditions[1]._value + "Schema).json(\"" + source.Value._conditions[1]._value + ".json\"");
						}

						stringBuilder.Append(");\n");
					}

					document.Add(stringBuilder.ToString());
				}

				//***************************************************************************
				// Edges
				//***************************************************************************

				// Per ogni edge crea la lista linkata
				foreach (Edge edge in edges)
				{

					// Per ogni elemento sorgente imposta il link di destinazione
					if (sources.ContainsKey(edge._source))
						sources[edge._source]._outputLink.Add(edge._target);

					// Per ogni elemento sorgente imposta il link di destinazione
					if (filters.ContainsKey(edge._source))
						filters[edge._source]._outputLink.Add(edge._target);

					// Per ogni elemento sorgente imposta il link di destinazione
					if (aggregates.ContainsKey(edge._source))
						aggregates[edge._source]._outputLink.Add(edge._target);

					// Per ogni elemento sorgente imposta il link di destinazione
					if (joins.ContainsKey(edge._source))
						joins[edge._source]._outputLink.Add(edge._target);

					// Per ogni elemento sorgente imposta il link di destinazione
					if (triggerEvents.ContainsKey(edge._source))
					{
						if ((triggerActions.ContainsKey(edge._target)) && (triggerActions[edge._target]._event_state == "on"))
							triggerEvents[edge._source]._triggerOn._outputLink.Add(edge._target);
						else if ((triggerActions.ContainsKey(edge._target)) && (triggerActions[edge._target]._event_state == "off"))
							triggerEvents[edge._source]._triggerOff._outputLink.Add(edge._target);
						else
						{
							triggerEvents[edge._source]._triggerOn._outputLink.Add("undef");
							triggerEvents[edge._source]._triggerOff._outputLink.Add("undef");
						}
					}

					// Per ogni elemento sorgente imposta il link di destinazione
					if (triggerActions.ContainsKey(edge._source))
						triggerActions[edge._source]._outputLink.Add(edge._target);

					// Per ogni elemento destinazione imposta il link di ingresso
					if (destinations.ContainsKey(edge._target))
						destinations[edge._target]._inputLink.Add(edge._source);

					// Per ogni elemento destinazione imposta il link di ingresso
					if (filters.ContainsKey(edge._target))
						filters[edge._target]._inputLink.Add(edge._source);

					// Per ogni elemento destinazione imposta il link di ingresso
					if (joins.ContainsKey(edge._target))
						joins[edge._target]._inputLink.Add(edge._source);
					
					// Per ogni elemento destinazione imposta il link di ingresso
					if (aggregates.ContainsKey(edge._target))
						aggregates[edge._target]._inputLink.Add(edge._source);

					// Per ogni elemento destinazione imposta il link di ingresso
					if (triggerEvents.ContainsKey(edge._target))
						triggerEvents[edge._target]._inputLink.Add(edge._source);

					// Per ogni elemento destinazione imposta il link di ingresso
					if (triggerActions.ContainsKey(edge._target))
						triggerActions[edge._target]._inputLink.Add(edge._source);
				}

				//***************************************************************************
				// Has destination
				//***************************************************************************

				// Destination 
				foreach (KeyValuePair<string, Filter> filter in filters)
				{
					if ((filter.Value._outputLink != null)&&(filter.Value._outputLink.Count >0))
						filter.Value._destination = FindDestination(filter.Value._outputLink[0]);
					else
						filter.Value._destination = "undef";
				}

				// Destination 
				foreach (KeyValuePair<string, Aggregate> aggregate in aggregates)
				{
					if ((aggregate.Value._outputLink != null)&&(aggregate.Value._outputLink.Count>0))
						aggregate.Value._destination = FindDestination(aggregate.Value._outputLink[0]);
					else
						aggregate.Value._destination = "undef";
				}

				// Destination 
				foreach (KeyValuePair<string, Join> joines in joins)
				{
					if ((joines.Value._outputLink != null) && (joines.Value._outputLink.Count > 0))
						joines.Value._destination = FindDestination(joines.Value._outputLink[0]);
					else
						joines.Value._destination = "undef";
				}

				// Destination 
				foreach (KeyValuePair<string, TriggerEvent> triggerEvent in triggerEvents)
				{
					if ((triggerEvent.Value._triggerOn._outputLink != null)&&(triggerEvent.Value._triggerOn._outputLink.Count>0))
						triggerEvent.Value._triggerOn._destination = FindDestination(triggerEvent.Value._triggerOn._outputLink[0]);
					else
						triggerEvent.Value._triggerOn._destination = "undef";

					if ((triggerEvent.Value._triggerOff._outputLink != null)&&(triggerEvent.Value._triggerOff._outputLink.Count>0))
						triggerEvent.Value._triggerOff._destination = FindDestination(triggerEvent.Value._triggerOff._outputLink[0]);
					else
						triggerEvent.Value._triggerOff._destination = "undef";
				}

				// Destination 
				foreach (KeyValuePair<string, TriggerAction> triggerAction in triggerActions)
				{
					if ((triggerAction.Value._outputLink != null)&&(triggerAction.Value._outputLink.Count>0))
						triggerAction.Value._destination = FindDestination(triggerAction.Value._outputLink[0]);
					else
						triggerAction.Value._destination = "undef";
				}

				//***************************************************************************
				// Has trigger Event
				//***************************************************************************

				// Destination 
				foreach (KeyValuePair<string, Source> source in sources)
				{
					if ((source.Value._outputLink != null) && (source.Value._outputLink.Count > 0))
						source.Value._triggerEvent = FindTriggerEvent(source.Value._outputLink[0]);
					else
						source.Value._triggerEvent = "undef";
				}

				// Destination 
				foreach (KeyValuePair<string, Filter> filter in filters)
				{
					if ((filter.Value._outputLink != null) && (filter.Value._outputLink.Count > 0))
						filter.Value._triggerEvent = FindTriggerEvent(filter.Value._outputLink[0]);
					else
						filter.Value._triggerEvent = "undef";
				}

				// Destination 
				foreach (KeyValuePair<string, Join> joines in joins)
				{
					if ((joines.Value._outputLink != null) && (joines.Value._outputLink.Count > 0))
						joines.Value._triggerEvent = FindTriggerEvent(joines.Value._outputLink[0]);
					else
						joines.Value._triggerEvent = "undef";
				}

				// Destination 
				foreach (KeyValuePair<string, Aggregate> aggregate in aggregates)
				{
					if ((aggregate.Value._outputLink != null) && (aggregate.Value._outputLink.Count > 0))
						aggregate.Value._triggerEvent = FindTriggerEvent(aggregate.Value._outputLink[0]);
					else
						aggregate.Value._triggerEvent = "undef";
				}

				//***************************************************************************
				// Has trigger Action
				//***************************************************************************

				// Destination 
				foreach (KeyValuePair<string, Source> source in sources)
				{
					if ((source.Value._outputLink != null) && (source.Value._outputLink.Count > 0))
						source.Value._triggerAction = FindTriggerActionRight(source.Value._outputLink[0]);
					else
						source.Value._triggerAction = "undef";
				}

				// Destination 
				foreach (KeyValuePair<string, Filter> filter in filters)
				{
					if ((filter.Value._outputLink != null) && (filter.Value._outputLink.Count > 0))
						filter.Value._triggerAction = FindTriggerActionRight(filter.Value._outputLink[0]);
					else
						filter.Value._triggerAction = "undef";
				}

				// Destination 
				foreach (KeyValuePair<string, Join> joines in joins)
				{
					if ((joines.Value._outputLink != null) && (joines.Value._outputLink.Count > 0))
						joines.Value._triggerAction = FindTriggerActionRight(joines.Value._outputLink[0]);
					else
						joines.Value._triggerAction = "undef";
				}

				// Destination 
				foreach (KeyValuePair<string, Aggregate> aggregate in aggregates)
				{
					if ((aggregate.Value._outputLink != null) && (aggregate.Value._outputLink.Count > 0))
						aggregate.Value._triggerAction = FindTriggerActionRight(aggregate.Value._outputLink[0]);
					else
						aggregate.Value._triggerAction = "undef";
				}

				//***************************************************************************
				// Has source
				//***************************************************************************

				// Source
				foreach (KeyValuePair<string, Filter> filter in filters)
				{
					if ((filter.Value._inputLink != null) && (filter.Value._inputLink.Count > 0))
						filter.Value._source = FindSource(filter.Value._inputLink[0]);
					else
						filter.Value._source = "undef";
				}
				// Source
				foreach (KeyValuePair<string, Join> joines in joins)
				{
					if ((joines.Value._inputLink != null) && (joines.Value._inputLink.Count > 0))
						joines.Value._source = FindSource(joines.Value._inputLink[0]);
					else
						joines.Value._source = "undef";
				}

				// Source 
				foreach (KeyValuePair<string, Aggregate> aggregate in aggregates)
				{
					if ((aggregate.Value._inputLink != null) && (aggregate.Value._inputLink.Count > 0))
						aggregate.Value._source = FindSource(aggregate.Value._inputLink[0]);
					else
						aggregate.Value._source = "undef";
				}

				// Source 
				foreach (KeyValuePair<string, TriggerEvent> triggerEvent in triggerEvents)
				{
					if ((triggerEvent.Value._inputLink != null) && (triggerEvent.Value._inputLink.Count > 0))
						triggerEvent.Value._source = FindSource(triggerEvent.Value._inputLink[0]);
					else
						triggerEvent.Value._source = "undef";
				}

				// Source 
				foreach (KeyValuePair<string, TriggerAction> triggerAction in triggerActions)
				{
					if ((triggerAction.Value._inputLink != null) && (triggerAction.Value._inputLink.Count > 0))
						triggerAction.Value._source = FindSource(triggerAction.Value._inputLink[0]);
					else
						triggerAction.Value._source = "undef";
				}

				// Search inside all filter
				foreach (KeyValuePair<string, Filter> filter in filters)
				{

					// String Builder
					StringBuilder stringBuilder = new StringBuilder();

					// If they are connected to source and destination
					if ((filter.Value._inputLink.Count > 0) && (filter.Value._outputLink.Count > 0) && (filter.Value._triggerAction == "undef"))
					{
						



							// Add filter syntax
							stringBuilder.Append("\tDataset<Row> " + filter.Value._id + " = " + filter.Value._inputLink[0] + ".filter((col(\"");

							// For each condition
							for (int j = 0; (j < filter.Value._conditions.Count && j <1); j++)
							{
								if (filter.Value._conditions[j]._operator == "LIKE")
								{

									// Example: like(data_name1, ".*tweet.*"))

									// Append conditions
									stringBuilder.Append(filter.Value._conditions[j]._attribute + "\").contains(\"" +
														 filter.Value._conditions[j]._value + "\"))");
								}

								else if (filter.Value._conditions[j]._operator == "RANGE")
								{


									// Example: range(latitude , 33.0, 37.0) 

									// Append conditions
									stringBuilder.Append(filter.Value._conditions[j]._attribute + "\").between(\"" + filter.Value._conditions[j]._value1 + "\" ,\"" +
														 filter.Value._conditions[j]._value2 + "\"))");
								}
								else if (filter.Value._conditions[j]._operator == "=")
								{

									// Append conditions
									stringBuilder.Append(filter.Value._conditions[j]._attribute + "\").equalTo(\"" +
														 filter.Value._conditions[j]._value + "\"))");
								}
								else if (filter.Value._conditions[j]._operator == ">")
								{

									// Append conditions
									stringBuilder.Append(filter.Value._conditions[j]._attribute + "\").gt(\"" +
														 filter.Value._conditions[j]._value + "\"))");
								}
								else if (filter.Value._conditions[j]._operator == "<")
								{

									// Append conditions
									stringBuilder.Append(filter.Value._conditions[j]._attribute + "\").lt(\"" +
														 filter.Value._conditions[j]._value + "\"))");
								}
								else if (filter.Value._conditions[j]._operator == ">=")
								{

									// Append conditions
									stringBuilder.Append(filter.Value._conditions[j]._attribute + "\").geq(\"" +
														 filter.Value._conditions[j]._value + "\"))");
								}
								else if (filter.Value._conditions[j]._operator == "<=")
								{

									// Append conditions
									stringBuilder.Append(filter.Value._conditions[j]._attribute + "\").leq(\"" +
														 filter.Value._conditions[j]._value + "\"))");
								}
								else if (filter.Value._conditions[j]._operator == "!=")
								{

									// Append conditions
									stringBuilder.Append(filter.Value._conditions[j]._attribute + "\").notEqual(\"" +
														 filter.Value._conditions[j]._value + "\"))");
								}
								else {
									stringBuilder.Append("\"))");
								}
							}

							// For each condition
							for (int j = 1; j < filter.Value._conditions.Count; j++)
							{

								if (filter.Value._conditions[j]._operator == "LIKE")
								{

									// Example: like(data_name1, ".*tweet.*"))

									// Append conditions
									stringBuilder.Append(".and(col(\"" + filter.Value._conditions[j]._attribute + "\").contains(\"" +
										filter.Value._conditions[j]._value + "\"))");
								}

								else if (filter.Value._conditions[j]._operator == "RANGE")
								{


									// Example: range(latitude , 33.0, 37.0) 

									// Append conditions
									stringBuilder.Append(".and(col(\"" + filter.Value._conditions[j]._attribute + "\").between(\"" + filter.Value._conditions[j]._value1 + "\" ,\"" +
														 filter.Value._conditions[j]._value2 + "\"))");
								}
								else if (filter.Value._conditions[j]._operator == "=")
								{

									// Append condition
									stringBuilder.Append(".and(col(\"" + filter.Value._conditions[j]._attribute + "\").equalTo(\"" +
														 filter.Value._conditions[j]._value + "\"))");
								}
								else if (filter.Value._conditions[j]._operator == ">")
								{

									// Append condition
									stringBuilder.Append(".and(col(\"" + filter.Value._conditions[j]._attribute + "\").gt(\"" +
														 filter.Value._conditions[j]._value + "\"))");
								}
								else if (filter.Value._conditions[j]._operator == "<")
								{

									// Append condition
									stringBuilder.Append(".and(col(\"" + filter.Value._conditions[j]._attribute + "\").lt(\"" +
														 filter.Value._conditions[j]._value + "\"))");
								}
								else if (filter.Value._conditions[j]._operator == ">=")
								{

									// Append condition
									stringBuilder.Append(".and(col(\"" + filter.Value._conditions[j]._attribute + "\").geq(\"" +
														 filter.Value._conditions[j]._value + "\"))");
								}
								else if (filter.Value._conditions[j]._operator == "<=")
								{

									// Append condition
									stringBuilder.Append(".and(col(\"" + filter.Value._conditions[j]._attribute + "\").leq(\"" +
														 filter.Value._conditions[j]._value + "\"))");
								}
								else if (filter.Value._conditions[j]._operator == "!=")
								{

									// Append condition
									stringBuilder.Append(".and(col(\"" + filter.Value._conditions[j]._attribute + "\").notEqual(\"" +
														 filter.Value._conditions[j]._value + "\"))");
								}
								else {
									stringBuilder.Append("\"))");
								}
							}

							// For each condition
							for (int j = 0; (j < filter.Value._orconditions.Count && j < 1); j++)
							{
								if (filter.Value._orconditions[j]._operator == "LIKE")
								{

									// Example: like(data_name1, ".*tweet.*"))

									// Append conditions
									stringBuilder.Append(filter.Value._orconditions[j]._attribute + "\").contains(\"" +
														 filter.Value._orconditions[j]._value + "\"))");
								}

								else if (filter.Value._orconditions[j]._operator == "RANGE")
								{


									// Example: range(latitude , 33.0, 37.0) 

									// Append conditions
									stringBuilder.Append(filter.Value._orconditions[j]._attribute + "\").between(\"" + filter.Value._orconditions[j]._value1 + "\" ,\"" +
														 filter.Value._orconditions[j]._value2 + "\"))");
								}
								else if (filter.Value._orconditions[j]._operator == "=")
								{

									// Append condition
									stringBuilder.Append(filter.Value._orconditions[j]._attribute + "\").equalTo(\"" +
														 filter.Value._orconditions[j]._value + "\"))");
								}
								else if (filter.Value._orconditions[j]._operator == ">")
								{

									// Append condition
									stringBuilder.Append(filter.Value._orconditions[j]._attribute + "\").gt(\"" +
														 filter.Value._orconditions[j]._value + "\"))");
								}
								else if (filter.Value._orconditions[j]._operator == "<")
								{

									// Append condition
									stringBuilder.Append(filter.Value._orconditions[j]._attribute + "\").lt(\"" +
														 filter.Value._orconditions[j]._value + "\"))");
								}
								else if (filter.Value._orconditions[j]._operator == ">=")
								{

									// Append condition
									stringBuilder.Append(filter.Value._orconditions[j]._attribute + "\").geq(\"" +
														 filter.Value._orconditions[j]._value + "\"))");
								}
								else if (filter.Value._orconditions[j]._operator == "<=")
								{

									// Append condition
									stringBuilder.Append(filter.Value._orconditions[j]._attribute + "\").leq(\"" +
														 filter.Value._orconditions[j]._value + "\"))");
								}
								else if (filter.Value._orconditions[j]._operator == "!=")
								{

									// Append condition
									stringBuilder.Append(filter.Value._orconditions[j]._attribute + "\").notEqual(\"" +
														 filter.Value._orconditions[j]._value + "\"))");
								}
								else {
									stringBuilder.Append("\"))");
								}
							}

							// For each condition
							for (int j = 1; j < filter.Value._orconditions.Count; j++)
							{


								if (filter.Value._orconditions[j]._operator == "LIKE")
								{

									// Example: like(data_name1, ".*tweet.*"))

									// Append conditions
									stringBuilder.Append(".or(col(\"" + filter.Value._orconditions[j]._attribute + "\").contains(\"" +
														 filter.Value._orconditions[j]._value + "\"))");
								}

								else if (filter.Value._orconditions[j]._operator == "RANGE")
								{


									// Example: range(latitude , 33.0, 37.0) 

									// Append conditions
									stringBuilder.Append(".or(col(\"" + filter.Value._orconditions[j]._attribute + "\").between(\"" + filter.Value._orconditions[j]._value1 + "\" ,\"" +
														 filter.Value._orconditions[j]._value2 + "\"))");
								}
								else if (filter.Value._orconditions[j]._operator == "=")
								{

									// Append condition
									stringBuilder.Append(".or(col(\"" + filter.Value._orconditions[j]._attribute + "\").equalTo(\"" +
														 filter.Value._orconditions[j]._value + "\"))");
								}
								else if (filter.Value._orconditions[j]._operator == ">")
								{

									// Append condition
									stringBuilder.Append(".or(col(\"" + filter.Value._orconditions[j]._attribute + "\").gt(\"" +
														 filter.Value._orconditions[j]._value + "\"))");
								}
								else if (filter.Value._orconditions[j]._operator == "<")
								{

									// Append condition
									stringBuilder.Append(".or(col(\"" + filter.Value._orconditions[j]._attribute + "\").lt(\"" +
														 filter.Value._orconditions[j]._value + "\"))");
								}
								else if (filter.Value._orconditions[j]._operator == ">=")
								{

									// Append condition
									stringBuilder.Append(".or(col(\"" + filter.Value._orconditions[j]._attribute + "\").geq(\"" +
														 filter.Value._orconditions[j]._value + "\"))");
								}
								else if (filter.Value._orconditions[j]._operator == "<=")
								{

									// Append condition
									stringBuilder.Append(".or(col(\"" + filter.Value._orconditions[j]._attribute + "\").leq(\"" +
														 filter.Value._orconditions[j]._value + "\"))");
								}
								else if (filter.Value._orconditions[j]._operator == "!=")
								{

									// Append condition
									stringBuilder.Append(".or(col(\"" + filter.Value._orconditions[j]._attribute + "\").notEqual(\"" +
														 filter.Value._orconditions[j]._value + "\"))");
								}
								else {
									stringBuilder.Append("\"))");
								}
							}

							// Close
							stringBuilder.Append(");\n");

							// Add to document
							document.Add(stringBuilder.ToString());


					}
				}
				// Search inside all Join
				foreach (KeyValuePair<string, Join> joines in joins)
				{
					Console.WriteLine("foreach join" + joines.Value._inputLink.Count);
					// String Builder
					StringBuilder stringBuilder = new StringBuilder();

					// If they are connected to source and destination
					if ((joines.Value._inputLink.Count > 0) && (joines.Value._outputLink.Count > 0))
					{

							// Add filter synta
							stringBuilder.Append("\tDataset<Row> " + joines.Value._id + " = " + joines.Value._inputLink[0] + ".join(" + joines.Value._inputLink[1] + ", " + joines.Value._inputLink[1] + ".col(\"" + joines.Value._join_schema[0]._property2
							                     + "\").equalTo("+ joines.Value._inputLink[0] + ".col(\""+ joines.Value._join_schema[0]._property + "\")), \"" + joines.Value._join_type + "\"");

							// Close
							stringBuilder.Append(");\n");

							// Add to document
							document.Add(stringBuilder.ToString());
					}
				}

				// Search inside all aggregate
				foreach (KeyValuePair<string, Aggregate> aggregate in aggregates)
				{

					// String Builder
					StringBuilder stringBuilder = new StringBuilder();

					// If they are connected to source and destination
					if ((aggregate.Value._inputLink.Count > 0) && (aggregate.Value._outputLink.Count > 0))
					{

						// Add filter syntax
						stringBuilder.Append("\tDataset<Row> " + aggregate.Value._id + " = " + aggregate.Value._inputLink[0] + ".groupBy(functions.window(" + aggregate.Value._inputLink[0] + ".col(\"time\"), \""+ aggregate.Value._settings._time_interval + " " + aggregate.Value._settings._time_unit +"\"))");

						stringBuilder.Append(".agg(functions.avg(\""+ aggregate.Value._settings._aggregate_data_name+"\").as(\"avg\"), functions.sum(\""+ aggregate.Value._settings._aggregate_data_name+"\").as(\"sum\"), functions.min(\""+ aggregate.Value._settings._aggregate_data_name+"\").as(\"min\"), functions.max(\""+ aggregate.Value._settings._aggregate_data_name+"\").as(\"max\"), functions.count(\""+ aggregate.Value._settings._aggregate_data_name+ "\").as(\"count\")");

						// For each condition
						for (int j = 0; j < aggregate.Value._conditions.Count; j++)
						{
							if (aggregate.Value._conditions[j]._namec != aggregate.Value._settings._aggregate_data_name)
							{
								stringBuilder.Append(", functions.first(\"" + aggregate.Value._conditions[j]._namec + "\").as(\"" + aggregate.Value._conditions[j]._namec + "\")");
							}
						}

						stringBuilder.Append(");\n");
						// Add to document
						document.Add(stringBuilder.ToString());
					}
				}

				document.Add("");

				// Search inside all trigger
				foreach (KeyValuePair<string, TriggerEvent> triggerEvent in triggerEvents)
				{

					// String Builder
					StringBuilder stringBuilder = new StringBuilder();

					// If they are connected to source and destination
					if (triggerEvent.Value._inputLink.Count > 0)
					{

						// Per ogni elemento destinazione imposta il link di ingresso
						if (sources.ContainsKey(triggerEvent.Value._source))
						{

							stringBuilder = new StringBuilder();

							if (triggerEvent.Value._triggerOn._destination != "undef")
							{

								// Add filter syntax
								stringBuilder.Append("\tif(" + triggerEvent.Value._inputLink[0] +  ".filter((col(\""  );

								// For each condition
								// Condizioni legate in AND
								for (int j = 0; j < triggerEvent.Value._triggerOn._conditions.Count; j++)
								{
									
									if (triggerEvent.Value._triggerOn._conditions[j]._operator == "RANGE") {
										// Append conditions
									stringBuilder.Append(triggerEvent.Value._triggerOn._conditions[j]._attribute + "\").between(\"" + triggerEvent.Value._triggerOn._conditions[j]._value1 + "\" ,\"" +
															 triggerEvent.Value._triggerOn._conditions[j]._value2 + "\"))");                              
									}
									else if(triggerEvent.Value._triggerOn._conditions[j]._operator == "LIKE"){
										// Append conditions
										stringBuilder.Append(triggerEvent.Value._triggerOn._conditions[j]._attribute + "\").contains(\"" +
														 triggerEvent.Value._triggerOn._conditions[j]._value + "\"))");
									}
									else if (triggerEvent.Value._triggerOn._conditions[j]._operator == "=")
									{

										// Append conditions
										stringBuilder.Append(triggerEvent.Value._triggerOn._conditions[j]._attribute + "\").equalTo(\"" +
															 triggerEvent.Value._triggerOn._conditions[j]._value + "\"))");
									}
									else if (triggerEvent.Value._triggerOn._conditions[j]._operator == ">")
									{

										// Append conditions
										stringBuilder.Append(triggerEvent.Value._triggerOn._conditions[j]._attribute + "\").gt(\"" +
															 triggerEvent.Value._triggerOn._conditions[j]._value + "\"))");
									}
									else if (triggerEvent.Value._triggerOn._conditions[j]._operator == "<")
									{

										// Append conditions
										stringBuilder.Append(triggerEvent.Value._triggerOn._conditions[j]._attribute + "\").lt(\"" +
															 triggerEvent.Value._triggerOn._conditions[j]._value + "\"))");
									}
									else if (triggerEvent.Value._triggerOn._conditions[j]._operator == ">=")
									{

										// Append conditions
										stringBuilder.Append(triggerEvent.Value._triggerOn._conditions[j]._attribute + "\").geq(\"" +
															 triggerEvent.Value._triggerOn._conditions[j]._value + "\"))");
									}
									else if (triggerEvent.Value._triggerOn._conditions[j]._operator == "<=")
									{

										// Append conditions
										stringBuilder.Append(triggerEvent.Value._triggerOn._conditions[j]._attribute + "\").leq(\"" +
															 triggerEvent.Value._triggerOn._conditions[j]._value + "\"))");
									}
									else if (triggerEvent.Value._triggerOn._conditions[j]._operator == "!=")
									{

										// Append conditions
										stringBuilder.Append(triggerEvent.Value._triggerOn._conditions[j]._attribute + "\").notEqual(\"" +
															 triggerEvent.Value._triggerOn._conditions[j]._value + "\"))");
									}
									else {
										stringBuilder.Append("\"))");
									}
									// Se non è l'ultimo elemento, lego in AND
									if (j != triggerEvent.Value._triggerOn._conditions.Count - 1){
										stringBuilder.Append(".and(col(\"");
									}
								}
								// Condizioni legate in OR
								for (int j = 0; j < triggerEvent.Value._triggerOn._orconditions.Count; j++)
								{

									if (triggerEvent.Value._triggerOn._orconditions[j]._operator == "RANGE")
									{
										// Append conditions
										stringBuilder.Append(triggerEvent.Value._triggerOn._orconditions[j]._attribute + "\").between(\"" + triggerEvent.Value._triggerOn._orconditions[j]._value1 + "\" ,\"" +
																 triggerEvent.Value._triggerOn._orconditions[j]._value2 + "\"))");
									}
									else if (triggerEvent.Value._triggerOn._orconditions[j]._operator == "LIKE")
									{
										// Append conditions
										stringBuilder.Append(triggerEvent.Value._triggerOn._orconditions[j]._attribute + "\").contains(\"" +
														 triggerEvent.Value._triggerOn._orconditions[j]._value + "\"))");
									}
									else if (triggerEvent.Value._triggerOn._orconditions[j]._operator == "=")
									{

										// Append conditions
										stringBuilder.Append(triggerEvent.Value._triggerOn._orconditions[j]._attribute + "\").equalTo(\"" +
															 triggerEvent.Value._triggerOn._orconditions[j]._value + "\"))");
									}
									else if (triggerEvent.Value._triggerOn._orconditions[j]._operator == ">")
									{

										// Append conditions
										stringBuilder.Append(triggerEvent.Value._triggerOn._orconditions[j]._attribute + "\").gt(\"" +
															 triggerEvent.Value._triggerOn._orconditions[j]._value + "\"))");
									}
									else if (triggerEvent.Value._triggerOn._orconditions[j]._operator == "<")
									{

										// Append conditions
										stringBuilder.Append(triggerEvent.Value._triggerOn._orconditions[j]._attribute + "\").lt(\"" +
															 triggerEvent.Value._triggerOn._orconditions[j]._value + "\"))");
									}
									else if (triggerEvent.Value._triggerOn._orconditions[j]._operator == ">=")
									{

										// Append conditions
										stringBuilder.Append(triggerEvent.Value._triggerOn._orconditions[j]._attribute + "\").geq(\"" +
															 triggerEvent.Value._triggerOn._orconditions[j]._value + "\"))");
									}
									else if (triggerEvent.Value._triggerOn._orconditions[j]._operator == "<=")
									{

										// Append conditions
										stringBuilder.Append(triggerEvent.Value._triggerOn._orconditions[j]._attribute + "\").leq(\"" +
															 triggerEvent.Value._triggerOn._orconditions[j]._value + "\"))");
									}
									else if (triggerEvent.Value._triggerOn._orconditions[j]._operator == "!=")
									{

										// Append conditions
										stringBuilder.Append(triggerEvent.Value._triggerOn._orconditions[j]._attribute + "\").notEqual(\"" +
															 triggerEvent.Value._triggerOn._orconditions[j]._value + "\"))");
									}
									else {
										stringBuilder.Append("\"))");
									}
									// Se non è l'ultimo elemento, lego in AND
									if (j != triggerEvent.Value._triggerOn._orconditions.Count - 1)
									{
										stringBuilder.Append(".or(col(\"");
									}
								}
								// End consition on
								stringBuilder.Append(").count() " + triggerEvent.Value._triggerOn._triggerCondition._expression + " " + triggerEvent.Value._triggerOn._triggerCondition._data_value  + ")");

								document.Add(stringBuilder.ToString());

							}



							if (triggerEvent.Value._triggerOn._destination != "undef")
							{
								stringBuilder = new StringBuilder();

								if ((triggerEvent.Value._triggerOn._outputLink.Count > 0) &&
									(triggerActions.ContainsKey(triggerEvent.Value._triggerOn._outputLink[0])))
								{

									stringBuilder.AppendLine("\t{");

									document.Add(stringBuilder.ToString());

									// Search inside all filter
									foreach (KeyValuePair<string, Filter> filter in filters)
									{

										// String Builder
										stringBuilder = new StringBuilder();

										// If they are connected to source and destination
										if ((filter.Value._inputLink.Count > 0) && (filter.Value._outputLink.Count > 0) && filter.Value._triggerAction.Contains("ton"))
										{
											Console.WriteLine("ton " + filter.Value._triggerAction);

												// Add filter syntax
												stringBuilder.Append("\tDataset<Row> " + filter.Value._id + " = " + filter.Value._inputLink[0] + ".filter((col(\"");

											// For each condition
											for (int j = 0; (j < filter.Value._conditions.Count && j < 1); j++)
											{
												if (filter.Value._conditions[j]._operator == "LIKE")
												{

													// Example: like(data_name1, ".*tweet.*"))

													// Append conditions
													stringBuilder.Append(filter.Value._conditions[j]._attribute + "\").contains(\"" +
																		 filter.Value._conditions[j]._value + "\"))");
												}

												else if (filter.Value._conditions[j]._operator == "RANGE")
												{


													// Example: range(latitude , 33.0, 37.0) 

													// Append conditions
													stringBuilder.Append(filter.Value._conditions[j]._attribute + "\").between(\"" + filter.Value._conditions[j]._value1 + "\" ,\"" +
																		 filter.Value._conditions[j]._value2 + "\"))");
												}
												else if (filter.Value._conditions[j]._operator == "=")
												{

													// Append conditions
													stringBuilder.Append(filter.Value._conditions[j]._attribute + "\").equalTo(\"" +
																		 filter.Value._conditions[j]._value + "\"))");
												}
												else if (filter.Value._conditions[j]._operator == ">")
												{

													// Append conditions
													stringBuilder.Append(filter.Value._conditions[j]._attribute + "\").gt(\"" +
																		 filter.Value._conditions[j]._value + "\"))");
												}
												else if (filter.Value._conditions[j]._operator == "<")
												{

													// Append conditions
													stringBuilder.Append(filter.Value._conditions[j]._attribute + "\").lt(\"" +
																		 filter.Value._conditions[j]._value + "\"))");
												}
												else if (filter.Value._conditions[j]._operator == ">=")
												{

													// Append conditions
													stringBuilder.Append(filter.Value._conditions[j]._attribute + "\").geq(\"" +
																		 filter.Value._conditions[j]._value + "\"))");
												}
												else if (filter.Value._conditions[j]._operator == "<=")
												{

													// Append conditions
													stringBuilder.Append(filter.Value._conditions[j]._attribute + "\").leq(\"" +
																		 filter.Value._conditions[j]._value + "\"))");
												}
												else if (filter.Value._conditions[j]._operator == "!=")
												{

													// Append conditions
													stringBuilder.Append(filter.Value._conditions[j]._attribute + "\").notEqual(\"" +
																		 filter.Value._conditions[j]._value + "\"))");
												}
												else {
													stringBuilder.Append("\"))");
												}
											}

											// For each condition
											for (int j = 1; j < filter.Value._conditions.Count; j++)
											{

												if (filter.Value._conditions[j]._operator == "LIKE")
												{

													// Example: like(data_name1, ".*tweet.*"))

													// Append conditions
													stringBuilder.Append(".and(col(\"" + filter.Value._conditions[j]._attribute + "\").contains(\"" +
														filter.Value._conditions[j]._value + "\"))");
												}

												else if (filter.Value._conditions[j]._operator == "RANGE")
												{


													// Example: range(latitude , 33.0, 37.0) 

													// Append conditions
													stringBuilder.Append(".and(col(\"" + filter.Value._conditions[j]._attribute + "\").between(\"" + filter.Value._conditions[j]._value1 + "\" ,\"" +
																		 filter.Value._conditions[j]._value2 + "\"))");
												}
												else if (filter.Value._conditions[j]._operator == "=")
												{

													// Append condition
													stringBuilder.Append(".and(col(\"" + filter.Value._conditions[j]._attribute + "\").equalTo(\"" +
																		 filter.Value._conditions[j]._value + "\"))");
												}
												else if (filter.Value._conditions[j]._operator == ">")
												{

													// Append condition
													stringBuilder.Append(".and(col(\"" + filter.Value._conditions[j]._attribute + "\").gt(\"" +
																		 filter.Value._conditions[j]._value + "\"))");
												}
												else if (filter.Value._conditions[j]._operator == "<")
												{

													// Append condition
													stringBuilder.Append(".and(col(\"" + filter.Value._conditions[j]._attribute + "\").lt(\"" +
																		 filter.Value._conditions[j]._value + "\"))");
												}
												else if (filter.Value._conditions[j]._operator == ">=")
												{

													// Append condition
													stringBuilder.Append(".and(col(\"" + filter.Value._conditions[j]._attribute + "\").geq(\"" +
																		 filter.Value._conditions[j]._value + "\"))");
												}
												else if (filter.Value._conditions[j]._operator == "<=")
												{

													// Append condition
													stringBuilder.Append(".and(col(\"" + filter.Value._conditions[j]._attribute + "\").leq(\"" +
																		 filter.Value._conditions[j]._value + "\"))");
												}
												else if (filter.Value._conditions[j]._operator == "!=")
												{

													// Append condition
													stringBuilder.Append(".and(col(\"" + filter.Value._conditions[j]._attribute + "\").notEqual(\"" +
																		 filter.Value._conditions[j]._value + "\"))");
												}
												else {
													stringBuilder.Append("\"))");
												}
											}

											// For each condition
											for (int j = 0; (j < filter.Value._orconditions.Count && j < 1); j++)
											{
												if (filter.Value._orconditions[j]._operator == "LIKE")
												{

													// Example: like(data_name1, ".*tweet.*"))

													// Append conditions
													stringBuilder.Append(filter.Value._orconditions[j]._attribute + "\").contains(\"" +
																		 filter.Value._orconditions[j]._value + "\"))");
												}

												else if (filter.Value._orconditions[j]._operator == "RANGE")
												{


													// Example: range(latitude , 33.0, 37.0) 

													// Append conditions
													stringBuilder.Append(filter.Value._orconditions[j]._attribute + "\").between(\"" + filter.Value._orconditions[j]._value1 + "\" ,\"" +
																		 filter.Value._orconditions[j]._value2 + "\"))");
												}
												else if (filter.Value._orconditions[j]._operator == "=")
												{

													// Append condition
													stringBuilder.Append(filter.Value._orconditions[j]._attribute + "\").equalTo(\"" +
																		 filter.Value._orconditions[j]._value + "\"))");
												}
												else if (filter.Value._orconditions[j]._operator == ">")
												{

													// Append condition
													stringBuilder.Append(filter.Value._orconditions[j]._attribute + "\").gt(\"" +
																		 filter.Value._orconditions[j]._value + "\"))");
												}
												else if (filter.Value._orconditions[j]._operator == "<")
												{

													// Append condition
													stringBuilder.Append(filter.Value._orconditions[j]._attribute + "\").lt(\"" +
																		 filter.Value._orconditions[j]._value + "\"))");
												}
												else if (filter.Value._orconditions[j]._operator == ">=")
												{

													// Append condition
													stringBuilder.Append(filter.Value._orconditions[j]._attribute + "\").geq(\"" +
																		 filter.Value._orconditions[j]._value + "\"))");
												}
												else if (filter.Value._orconditions[j]._operator == "<=")
												{

													// Append condition
													stringBuilder.Append(filter.Value._orconditions[j]._attribute + "\").leq(\"" +
																		 filter.Value._orconditions[j]._value + "\"))");
												}
												else if (filter.Value._orconditions[j]._operator == "!=")
												{

													// Append condition
													stringBuilder.Append(filter.Value._orconditions[j]._attribute + "\").notEqual(\"" +
																		 filter.Value._orconditions[j]._value + "\"))");
												}
												else {
													stringBuilder.Append("\"))");
												}
											}

											// For each condition
											for (int j = 1; j < filter.Value._orconditions.Count; j++)
											{


												if (filter.Value._orconditions[j]._operator == "LIKE")
												{

													// Example: like(data_name1, ".*tweet.*"))

													// Append conditions
													stringBuilder.Append(".or(col(\"" + filter.Value._orconditions[j]._attribute + "\").contains(\"" +
																		 filter.Value._orconditions[j]._value + "\"))");
												}

												else if (filter.Value._orconditions[j]._operator == "RANGE")
												{


													// Example: range(latitude , 33.0, 37.0) 

													// Append conditions
													stringBuilder.Append(".or(col(\"" + filter.Value._orconditions[j]._attribute + "\").between(\"" + filter.Value._orconditions[j]._value1 + "\" ,\"" +
																		 filter.Value._orconditions[j]._value2 + "\"))");
												}
												else if (filter.Value._orconditions[j]._operator == "=")
												{

													// Append condition
													stringBuilder.Append(".or(col(\"" + filter.Value._orconditions[j]._attribute + "\").equalTo(\"" +
																		 filter.Value._orconditions[j]._value + "\"))");
												}
												else if (filter.Value._orconditions[j]._operator == ">")
												{

													// Append condition
													stringBuilder.Append(".or(col(\"" + filter.Value._orconditions[j]._attribute + "\").gt(\"" +
																		 filter.Value._orconditions[j]._value + "\"))");
												}
												else if (filter.Value._orconditions[j]._operator == "<")
												{

													// Append condition
													stringBuilder.Append(".or(col(\"" + filter.Value._orconditions[j]._attribute + "\").lt(\"" +
																		 filter.Value._orconditions[j]._value + "\"))");
												}
												else if (filter.Value._orconditions[j]._operator == ">=")
												{

													// Append condition
													stringBuilder.Append(".or(col(\"" + filter.Value._orconditions[j]._attribute + "\").geq(\"" +
																		 filter.Value._orconditions[j]._value + "\"))");
												}
												else if (filter.Value._orconditions[j]._operator == "<=")
												{

													// Append condition
													stringBuilder.Append(".or(col(\"" + filter.Value._orconditions[j]._attribute + "\").leq(\"" +
																		 filter.Value._orconditions[j]._value + "\"))");
												}
												else if (filter.Value._orconditions[j]._operator == "!=")
												{

													// Append condition
													stringBuilder.Append(".or(col(\"" + filter.Value._orconditions[j]._attribute + "\").notEqual(\"" +
																		 filter.Value._orconditions[j]._value + "\"))");
												}
												else {
													stringBuilder.Append("\"))");
												}
											}

											// Close
											stringBuilder.Append(");\n");

											// Add to document
											document.Add(stringBuilder.ToString());
										}

										// Search inside all aggregate
										foreach (KeyValuePair<string, Aggregate> aggregate in aggregates)
										{

											// String Builder
											stringBuilder = new StringBuilder();

											// If they are connected to source and destination
											if ((aggregate.Value._inputLink.Count > 0) && (aggregate.Value._outputLink.Count > 0))
											{

													// Add filter syntax
													stringBuilder.Append("\tDataset<Row> " + aggregate.Value._id + " = " + aggregate.Value._inputLink[0] + ".groupBy((col(\"");


													// Append conditions
													stringBuilder.Append("time(" + 
														", " + aggregate.Value._settings._time_interval +
														", " + aggregate.Value._settings._time_unit + ")" +
														", space)");

													// Close
													stringBuilder.Append(")");

													// Add to document
													document.Add(stringBuilder.ToString());
					
											}
										}
									}
									stringBuilder = new StringBuilder();
									stringBuilder.Append("\t}\n");

									document.Add(stringBuilder.ToString());

								}
							}
							stringBuilder = new StringBuilder();

							// Add filter syntax
							if (triggerEvent.Value._triggerOff._destination != "undef")
							{

								// Add filter syntax
								stringBuilder.Append("\telse if(" + triggerEvent.Value._inputLink[0] + ".filter((col(\"");


								// For each condition
								// Condizioni legate in AND
								for (int j = 0; j < triggerEvent.Value._triggerOff._conditions.Count; j++)
								{

									if (triggerEvent.Value._triggerOff._conditions[j]._operator == "RANGE")
									{
										// Append conditions
										stringBuilder.Append(triggerEvent.Value._triggerOff._conditions[j]._attribute + "\").between(\"" + triggerEvent.Value._triggerOff._conditions[j]._value1 + "\" ,\"" +
																 triggerEvent.Value._triggerOff._conditions[j]._value2 + "\"))");
									}
									else if (triggerEvent.Value._triggerOff._conditions[j]._operator == "LIKE")
									{
										// Append conditions
										stringBuilder.Append(triggerEvent.Value._triggerOff._conditions[j]._attribute + "\").contains(\"" +
														 triggerEvent.Value._triggerOff._conditions[j]._value + "\"))");
									}
									else if (triggerEvent.Value._triggerOff._conditions[j]._operator == "=")
									{

										// Append conditions
										stringBuilder.Append(triggerEvent.Value._triggerOff._conditions[j]._attribute + "\").equalTo(\"" +
															 triggerEvent.Value._triggerOff._conditions[j]._value + "\"))");
									}
									else if (triggerEvent.Value._triggerOff._conditions[j]._operator == ">")
									{

										// Append conditions
										stringBuilder.Append(triggerEvent.Value._triggerOff._conditions[j]._attribute + "\").gt(\"" +
															 triggerEvent.Value._triggerOff._conditions[j]._value + "\"))");
									}
									else if (triggerEvent.Value._triggerOff._conditions[j]._operator == "<")
									{

										// Append conditions
										stringBuilder.Append(triggerEvent.Value._triggerOff._conditions[j]._attribute + "\").lt(\"" +
															 triggerEvent.Value._triggerOff._conditions[j]._value + "\"))");
									}
									else if (triggerEvent.Value._triggerOff._conditions[j]._operator == ">=")
									{

										// Append conditions
										stringBuilder.Append(triggerEvent.Value._triggerOff._conditions[j]._attribute + "\").geq(\"" +
															 triggerEvent.Value._triggerOff._conditions[j]._value + "\"))");
									}
									else if (triggerEvent.Value._triggerOff._conditions[j]._operator == "<=")
									{

										// Append conditions
										stringBuilder.Append(triggerEvent.Value._triggerOff._conditions[j]._attribute + "\").leq(\"" +
															 triggerEvent.Value._triggerOff._conditions[j]._value + "\"))");
									}
									else if (triggerEvent.Value._triggerOff._conditions[j]._operator == "!=")
									{

										// Append conditions
										stringBuilder.Append(triggerEvent.Value._triggerOff._conditions[j]._attribute + "\").notEqual(\"" +
															 triggerEvent.Value._triggerOff._conditions[j]._value + "\"))");
									}
									else {
										stringBuilder.Append("\"))");
									}
									// Se non è l'ultimo elemento, lego in AND
									if (j != triggerEvent.Value._triggerOff._conditions.Count - 1)
									{
										stringBuilder.Append(".and(col(\"");
									}
								}
								// Condizioni legate in OR
								for (int j = 0; j < triggerEvent.Value._triggerOff._orconditions.Count; j++)
								{

									if (triggerEvent.Value._triggerOff._conditions[j]._operator == "RANGE")
									{
										// Append conditions
										stringBuilder.Append(triggerEvent.Value._triggerOff._orconditions[j]._attribute + "\").between(\"" + triggerEvent.Value._triggerOff._orconditions[j]._value1 + "\" ,\"" +
																 triggerEvent.Value._triggerOff._orconditions[j]._value2 + "\"))");
									}
									else if (triggerEvent.Value._triggerOff._orconditions[j]._operator == "LIKE")
									{
										// Append conditions
										stringBuilder.Append(triggerEvent.Value._triggerOff._orconditions[j]._attribute + "\").contains(\"" +
														 triggerEvent.Value._triggerOff._orconditions[j]._value + "\"))");
									}
									else if (triggerEvent.Value._triggerOff._orconditions[j]._operator == "=")
									{

										// Append conditions
										stringBuilder.Append(triggerEvent.Value._triggerOff._orconditions[j]._attribute + "\").equalTo(\"" +
															 triggerEvent.Value._triggerOff._orconditions[j]._value + "\"))");
									}
									else if (triggerEvent.Value._triggerOff._orconditions[j]._operator == ">")
									{

										// Append conditions
										stringBuilder.Append(triggerEvent.Value._triggerOff._orconditions[j]._attribute + "\").gt(\"" +
															 triggerEvent.Value._triggerOff._orconditions[j]._value + "\"))");
									}
									else if (triggerEvent.Value._triggerOff._orconditions[j]._operator == "<")
									{

										// Append conditions
										stringBuilder.Append(triggerEvent.Value._triggerOff._orconditions[j]._attribute + "\").lt(\"" +
															 triggerEvent.Value._triggerOff._orconditions[j]._value + "\"))");
									}
									else if (triggerEvent.Value._triggerOff._orconditions[j]._operator == ">=")
									{

										// Append conditions
										stringBuilder.Append(triggerEvent.Value._triggerOff._orconditions[j]._attribute + "\").geq(\"" +
															 triggerEvent.Value._triggerOff._orconditions[j]._value + "\"))");
									}
									else if (triggerEvent.Value._triggerOff._orconditions[j]._operator == "<=")
									{

										// Append conditions
										stringBuilder.Append(triggerEvent.Value._triggerOff._orconditions[j]._attribute + "\").leq(\"" +
															 triggerEvent.Value._triggerOff._orconditions[j]._value + "\"))");
									}
									else if (triggerEvent.Value._triggerOff._orconditions[j]._operator == "!=")
									{

										// Append conditions
										stringBuilder.Append(triggerEvent.Value._triggerOff._orconditions[j]._attribute + "\").notEqual(\"" +
															 triggerEvent.Value._triggerOff._orconditions[j]._value + "\"))");
									}
									else {
										stringBuilder.Append("\"))");
									}
									// Se non è l'ultimo elemento, lego in AND
									if (j != triggerEvent.Value._triggerOff._orconditions.Count - 1)
									{
										stringBuilder.Append(".or(col(\"");
									}
								}
								// End consition on
								stringBuilder.Append(").count() " + triggerEvent.Value._triggerOff._triggerCondition._expression + " " + triggerEvent.Value._triggerOff._triggerCondition._data_value + ")");

								document.Add(stringBuilder.ToString());

							}

							document.Add("");

							if (triggerEvent.Value._triggerOff._destination != "undef")
							{
								stringBuilder = new StringBuilder();

								if ((triggerEvent.Value._triggerOff._outputLink.Count > 0) &&
									(triggerActions.ContainsKey(triggerEvent.Value._triggerOff._outputLink[0])))
								{

									stringBuilder.AppendLine("\t{");

									document.Add(stringBuilder.ToString());

									// Search inside all filter
									foreach (KeyValuePair<string, Filter> filter in filters)
									{

										// String Builder
										stringBuilder = new StringBuilder();

										// If they are connected to source and destination
										if ((filter.Value._inputLink.Count > 0) && (filter.Value._outputLink.Count > 0) && filter.Value._triggerAction.Contains("toff"))
										{
											Console.WriteLine("off " + filter.Value._triggerAction);

											// Add filter syntax
											stringBuilder.Append("\tDataset<Row> " + filter.Value._id + " = " + filter.Value._inputLink[0] + ".filter((col(\"");

											// For each condition
											for (int j = 0; (j < filter.Value._conditions.Count && j < 1); j++)
											{
												if (filter.Value._conditions[j]._operator == "LIKE")
												{

													// Example: like(data_name1, ".*tweet.*"))

													// Append conditions
													stringBuilder.Append(filter.Value._conditions[j]._attribute + "\").contains(\"" +
																		 filter.Value._conditions[j]._value + "\"))");
												}

												else if (filter.Value._conditions[j]._operator == "RANGE")
												{


													// Example: range(latitude , 33.0, 37.0) 

													// Append conditions
													stringBuilder.Append(filter.Value._conditions[j]._attribute + "\").between(\"" + filter.Value._conditions[j]._value1 + "\" ,\"" +
																		 filter.Value._conditions[j]._value2 + "\"))");
												}
												else if (filter.Value._conditions[j]._operator == "=")
												{

													// Append conditions
													stringBuilder.Append(filter.Value._conditions[j]._attribute + "\").equalTo(\"" +
																		 filter.Value._conditions[j]._value + "\"))");
												}
												else if (filter.Value._conditions[j]._operator == ">")
												{

													// Append conditions
													stringBuilder.Append(filter.Value._conditions[j]._attribute + "\").gt(\"" +
																		 filter.Value._conditions[j]._value + "\"))");
												}
												else if (filter.Value._conditions[j]._operator == "<")
												{

													// Append conditions
													stringBuilder.Append(filter.Value._conditions[j]._attribute + "\").lt(\"" +
																		 filter.Value._conditions[j]._value + "\"))");
												}
												else if (filter.Value._conditions[j]._operator == ">=")
												{

													// Append conditions
													stringBuilder.Append(filter.Value._conditions[j]._attribute + "\").geq(\"" +
																		 filter.Value._conditions[j]._value + "\"))");
												}
												else if (filter.Value._conditions[j]._operator == "<=")
												{

													// Append conditions
													stringBuilder.Append(filter.Value._conditions[j]._attribute + "\").leq(\"" +
																		 filter.Value._conditions[j]._value + "\"))");
												}
												else if (filter.Value._conditions[j]._operator == "!=")
												{

													// Append conditions
													stringBuilder.Append(filter.Value._conditions[j]._attribute + "\").notEqual(\"" +
																		 filter.Value._conditions[j]._value + "\"))");
												}
												else {
													stringBuilder.Append("\"))");
												}
											}

											// For each condition
											for (int j = 1; j < filter.Value._conditions.Count; j++)
											{

												if (filter.Value._conditions[j]._operator == "LIKE")
												{

													// Example: like(data_name1, ".*tweet.*"))

													// Append conditions
													stringBuilder.Append(".and(col(\"" + filter.Value._conditions[j]._attribute + "\").contains(\"" +
														filter.Value._conditions[j]._value + "\"))");
												}

												else if (filter.Value._conditions[j]._operator == "RANGE")
												{


													// Example: range(latitude , 33.0, 37.0) 

													// Append conditions
													stringBuilder.Append(".and(col(\"" + filter.Value._conditions[j]._attribute + "\").between(\"" + filter.Value._conditions[j]._value1 + "\" ,\"" +
																		 filter.Value._conditions[j]._value2 + "\"))");
												}
												else if (filter.Value._conditions[j]._operator == "=")
												{

													// Append condition
													stringBuilder.Append(".and(col(\"" + filter.Value._conditions[j]._attribute + "\").equalTo(\"" +
																		 filter.Value._conditions[j]._value + "\"))");
												}
												else if (filter.Value._conditions[j]._operator == ">")
												{

													// Append condition
													stringBuilder.Append(".and(col(\"" + filter.Value._conditions[j]._attribute + "\").gt(\"" +
																		 filter.Value._conditions[j]._value + "\"))");
												}
												else if (filter.Value._conditions[j]._operator == "<")
												{

													// Append condition
													stringBuilder.Append(".and(col(\"" + filter.Value._conditions[j]._attribute + "\").lt(\"" +
																		 filter.Value._conditions[j]._value + "\"))");
												}
												else if (filter.Value._conditions[j]._operator == ">=")
												{

													// Append condition
													stringBuilder.Append(".and(col(\"" + filter.Value._conditions[j]._attribute + "\").geq(\"" +
																		 filter.Value._conditions[j]._value + "\"))");
												}
												else if (filter.Value._conditions[j]._operator == "<=")
												{

													// Append condition
													stringBuilder.Append(".and(col(\"" + filter.Value._conditions[j]._attribute + "\").leq(\"" +
																		 filter.Value._conditions[j]._value + "\"))");
												}
												else if (filter.Value._conditions[j]._operator == "!=")
												{

													// Append condition
													stringBuilder.Append(".and(col(\"" + filter.Value._conditions[j]._attribute + "\").notEqual(\"" +
																		 filter.Value._conditions[j]._value + "\"))");
												}
												else {
													stringBuilder.Append("\"))");
												}
											}

											// For each condition
											for (int j = 0; (j < filter.Value._orconditions.Count && j < 1); j++)
											{
												if (filter.Value._orconditions[j]._operator == "LIKE")
												{

													// Example: like(data_name1, ".*tweet.*"))

													// Append conditions
													stringBuilder.Append(filter.Value._orconditions[j]._attribute + "\").contains(\"" +
																		 filter.Value._orconditions[j]._value + "\"))");
												}
												else if (filter.Value._orconditions[j]._operator == "RANGE")
												{


													// Example: range(latitude , 33.0, 37.0) 

													// Append conditions
													stringBuilder.Append(filter.Value._orconditions[j]._attribute + "\").between(\"" + filter.Value._orconditions[j]._value1 + "\" ,\"" +
																		 filter.Value._orconditions[j]._value2 + "\"))");
												}
												else if (filter.Value._orconditions[j]._operator == "=")
												{

													// Append condition
													stringBuilder.Append(filter.Value._orconditions[j]._attribute + "\").equalTo(\"" +
																		 filter.Value._orconditions[j]._value + "\"))");
												}
												else if (filter.Value._orconditions[j]._operator == ">")
												{

													// Append condition
													stringBuilder.Append(filter.Value._orconditions[j]._attribute + "\").gt(\"" +
																		 filter.Value._orconditions[j]._value + "\"))");
												}
												else if (filter.Value._orconditions[j]._operator == "<")
												{

													// Append condition
													stringBuilder.Append(filter.Value._orconditions[j]._attribute + "\").lt(\"" +
																		 filter.Value._orconditions[j]._value + "\"))");
												}
												else if (filter.Value._orconditions[j]._operator == ">=")
												{

													// Append condition
													stringBuilder.Append(filter.Value._orconditions[j]._attribute + "\").geq(\"" +
																		 filter.Value._orconditions[j]._value + "\"))");
												}
												else if (filter.Value._orconditions[j]._operator == "<=")
												{

													// Append condition
													stringBuilder.Append(filter.Value._orconditions[j]._attribute + "\").leq(\"" +
																		 filter.Value._orconditions[j]._value + "\"))");
												}
												else if (filter.Value._orconditions[j]._operator == "!=")
												{

													// Append condition
													stringBuilder.Append(filter.Value._orconditions[j]._attribute + "\").notEqual(\"" +
																		 filter.Value._orconditions[j]._value + "\"))");
												}
												else {
													stringBuilder.Append("\"))");
												}
											}

											// For each condition
											for (int j = 1; j < filter.Value._orconditions.Count; j++)
											{


												if (filter.Value._orconditions[j]._operator == "LIKE")
												{

													// Example: like(data_name1, ".*tweet.*"))

													// Append conditions
													stringBuilder.Append(".or(col(\"" + filter.Value._orconditions[j]._attribute + "\").contains(\"" +
																		 filter.Value._orconditions[j]._value + "\"))");
												}

												else if (filter.Value._orconditions[j]._operator == "RANGE")
												{


													// Example: range(latitude , 33.0, 37.0) 

													// Append conditions
													stringBuilder.Append(".or(col(\"" + filter.Value._orconditions[j]._attribute + "\").between(\"" + filter.Value._orconditions[j]._value1 + "\" ,\"" +
																		 filter.Value._orconditions[j]._value2 + "\"))");
												}
												else if (filter.Value._orconditions[j]._operator == "=")
												{

													// Append condition
													stringBuilder.Append(".or(col(\"" + filter.Value._orconditions[j]._attribute + "\").equalTo(\"" +
																		 filter.Value._orconditions[j]._value + "\"))");
												}
												else if (filter.Value._orconditions[j]._operator == ">")
												{

													// Append condition
													stringBuilder.Append(".or(col(\"" + filter.Value._orconditions[j]._attribute + "\").gt(\"" +
																		 filter.Value._orconditions[j]._value + "\"))");
												}
												else if (filter.Value._orconditions[j]._operator == "<")
												{

													// Append condition
													stringBuilder.Append(".or(col(\"" + filter.Value._orconditions[j]._attribute + "\").lt(\"" +
																		 filter.Value._orconditions[j]._value + "\"))");
												}
												else if (filter.Value._orconditions[j]._operator == ">=")
												{

													// Append condition
													stringBuilder.Append(".or(col(\"" + filter.Value._orconditions[j]._attribute + "\").geq(\"" +
																		 filter.Value._orconditions[j]._value + "\"))");
												}
												else if (filter.Value._orconditions[j]._operator == "<=")
												{

													// Append condition
													stringBuilder.Append(".or(col(\"" + filter.Value._orconditions[j]._attribute + "\").leq(\"" +
																		 filter.Value._orconditions[j]._value + "\"))");
												}
												else if (filter.Value._orconditions[j]._operator == "!=")
												{

													// Append condition
													stringBuilder.Append(".or(col(\"" + filter.Value._orconditions[j]._attribute + "\").notEqual(\"" +
																		 filter.Value._orconditions[j]._value + "\"))");
												}
												else {
													stringBuilder.Append("\"))");
												}
											}

											// Close
											stringBuilder.Append(");\n");

											// Add to document
											document.Add(stringBuilder.ToString());
										}


										// Search inside all aggregate
										foreach (KeyValuePair<string, Aggregate> aggregate in aggregates)
										{

											// String Builder
											stringBuilder = new StringBuilder();

											// If they are connected to source and destination
											if ((aggregate.Value._inputLink.Count > 0) && (aggregate.Value._outputLink.Count > 0))
											{

												// Add filter syntax
												stringBuilder.Append("\tDataset<Row> " + aggregate.Value._id + " = " + aggregate.Value._inputLink[0] + ".groupBy((col(\"");


												// Append conditions
												stringBuilder.Append("time(" +
													", " + aggregate.Value._settings._time_interval +
													", " + aggregate.Value._settings._time_unit + ")" +
													", space)");

												// Close
												stringBuilder.Append(")");

												// Add to document
												document.Add(stringBuilder.ToString());

											}
										}
									}
									stringBuilder = new StringBuilder();
									stringBuilder.Append("\t}\n");

									document.Add(stringBuilder.ToString());

								}
							}
						}
					}
				}

				// End of bloom
				document.Add("\t}\n}");

				return true;

			}
			catch (Exception es)
			{

				// Error message
				Console.WriteLine("Error in DSN generation: " + es.Message);
				return false;
			}
		}

		/// <summary>
		/// Search for the destination
		/// </summary>
		/// <param name="link">Id next link</param>
		/// <returns></returns>
		string FindDestination(string outputLink)
		{

			// Attraversando la struttura cerca un punto destinazione
			while(true)
			{

				// Se non ci sono altri riferimenti 
				if (outputLink == null)
				{
					return "undef";
				}

				// Naviga attraverso la destinazione
				else if (destinations.ContainsKey(outputLink))
				{
					return outputLink;
				}

				// Naviga attraverso la sorgente
				else if (sources.ContainsKey(outputLink))
				{
					if (sources[outputLink]._outputLink.Count > 0)
						outputLink = sources[outputLink]._outputLink[0];
					else
						outputLink = null;
				}

				// Naviga attraverso il nodo filter
				else if (filters.ContainsKey(outputLink))
				{
					if (filters[outputLink]._outputLink.Count > 0)
						outputLink = filters[outputLink]._outputLink[0];
					else
						outputLink = null;
				}

				// Naviga attraverso l'aggregate
				else if (aggregates.ContainsKey(outputLink))
				{
					if (aggregates[outputLink]._outputLink.Count > 0)
						outputLink = aggregates[outputLink]._outputLink[0];
					else
						outputLink = null;
				}

				// Naviga attraverso il trigger events
				else if (triggerEvents.ContainsKey(outputLink))
				{
					if (triggerEvents[outputLink]._triggerOn._outputLink.Count > 0)
						outputLink = triggerEvents[outputLink]._triggerOn._outputLink[0];

					else if (triggerEvents[outputLink]._triggerOff._outputLink.Count > 0)
						outputLink = triggerEvents[outputLink]._triggerOff._outputLink[0];

					else
						outputLink = null;
				}

				// Naviga attraverso l'aggregate
				else if (triggerActions.ContainsKey(outputLink))
				{
					if (triggerActions[outputLink]._outputLink.Count > 0)
						outputLink = triggerActions[outputLink]._outputLink[0];
					else
						outputLink = null;
				}

				// Ogni altra condizione
				else
				{
					outputLink = null;
				}
			}
		}

		/// <summary>
		/// Search for the destination
		/// </summary>
		/// <param name="link">Id next link</param>
		/// <returns></returns>
		string FindTriggerEvent(string outputLink)
		{

			// Attraversando la struttura cerca un punto destinazione
			while (true)
			{

				// Se non ci sono altri riferimenti 
				if (outputLink == null)
				{
					return "undef";
				}

				// Naviga attraverso la destinazione
				else if (destinations.ContainsKey(outputLink))
				{
					outputLink = null;
				}

				// Naviga attraverso la sorgente
				else if (sources.ContainsKey(outputLink))
				{
					if (sources[outputLink]._outputLink.Count > 0)
						outputLink = sources[outputLink]._outputLink[0];
					else
						outputLink = null;
				}

				// Naviga attraverso il nodo filter
				else if (filters.ContainsKey(outputLink))
				{
					if (filters[outputLink]._outputLink.Count > 0)
						outputLink = filters[outputLink]._outputLink[0];
					else
						outputLink = null;
				}

				// Naviga attraverso l'aggregate
				else if (aggregates.ContainsKey(outputLink))
				{
					if (aggregates[outputLink]._outputLink.Count > 0)
						outputLink = aggregates[outputLink]._outputLink[0];
					else
						outputLink = null;
				}

				// Naviga attraverso il trigger events
				else if (triggerEvents.ContainsKey(outputLink))
				{
					return triggerEvents[outputLink]._id;
				}

				// Naviga attraverso l'aggregate
				else if (triggerActions.ContainsKey(outputLink))
				{
					outputLink = null;
				}

				// Ogni altra condizione
				else
				{
					outputLink = null;
				}
			}
		}

		/// <summary>
		/// Search for the destination
		/// </summary>
		/// <param name="link">Id next link</param>
		/// <returns></returns>
		string FindTriggerActionRight(string outputLink)
		{

			// Attraversando la struttura cerca un punto destinazione
			while (true)
			{

				// Se non ci sono altri riferimenti 
				if (outputLink == null)
				{
					return "undef";
				}

				// Naviga attraverso la destinazione
				else if (destinations.ContainsKey(outputLink))
				{
					outputLink = null;
				}

				// Naviga attraverso la sorgente
				else if (sources.ContainsKey(outputLink))
				{
					if (sources[outputLink]._outputLink.Count > 0)
						outputLink = sources[outputLink]._outputLink[0];
					else
						outputLink = null;
				}

				// Naviga attraverso il nodo filter
				else if (filters.ContainsKey(outputLink))
				{
					if (filters[outputLink]._outputLink.Count > 0)
						outputLink = filters[outputLink]._outputLink[0];
					else
						outputLink = null;
				}

				// Naviga attraverso l'aggregate
				else if (aggregates.ContainsKey(outputLink))
				{
					if (aggregates[outputLink]._outputLink.Count > 0)
						outputLink = aggregates[outputLink]._outputLink[0];
					else
						outputLink = null;
				}

				// Naviga attraverso il trigger events
				else if (triggerEvents.ContainsKey(outputLink))
				{
					outputLink = null;
				}

				// Naviga attraverso l'aggregate
				else if (triggerActions.ContainsKey(outputLink))
				{
					return outputLink;
				}

				// Ogni altra condizione
				else
				{
					outputLink = null;
				}
			}
		}

		/// <summary>
		/// Search for the destination
		/// </summary>
		/// <param name="link">Id next link</param>
		/// <returns></returns>
		string FindSource(string inputLink)
		{

			// Attraversando la struttura cerca un punto destinazione
			while (true)
			{

				// Se non ci sono altri riferimenti 
				if (inputLink == null)
				{
					return "undef";
				}

				// Naviga attraverso la destinazione
				else if (sources.ContainsKey(inputLink))
				{
					return inputLink;
				}

				// Naviga attraverso la sorgente
				else if (destinations.ContainsKey(inputLink))
				{
					if (destinations[inputLink]._inputLink.Count > 0)
						inputLink = destinations[inputLink]._inputLink[0];
					else
						inputLink = null;
				}

				// Naviga attraverso il nodo filter
				else if (filters.ContainsKey(inputLink))
				{
					if (filters[inputLink]._outputLink.Count > 0)
					{
						inputLink = filters[inputLink]._inputLink[0];
					}
					else
						inputLink = null;
				}

				// Naviga attraverso l'aggregate
				else if (aggregates.ContainsKey(inputLink))
				{
					if (aggregates[inputLink]._inputLink.Count > 0)
						inputLink = aggregates[inputLink]._inputLink[0];
					else
						inputLink = null;
				}

				// Naviga attraverso il trigger events
				else if (triggerEvents.ContainsKey(inputLink))
				{
					if (triggerEvents[inputLink]._inputLink.Count > 0)
						inputLink = triggerEvents[inputLink]._inputLink[0];
					else
						inputLink = null;
				}

				// Naviga attraverso il trigger events
				else if (triggerActions.ContainsKey(inputLink))
				{
					if (triggerActions[inputLink]._inputLink.Count > 0)
						inputLink = triggerActions[inputLink]._inputLink[0];
					else
						inputLink = null;
				}

				// Ogni altra condizione
				else
				{
					inputLink = null;
				}
			}
		}

		/// <summary>
		/// Search for the destination
		/// </summary>
		/// <param name="link">Id next link</param>
		/// <returns></returns>
		string FindTriggerActionLeft(string inputLink)
		{

			// Attraversando la struttura cerca un punto destinazione
			while (true)
			{

				// Se non ci sono altri riferimenti 
				if (inputLink == null)
				{
					return "undef";
				}

				// Naviga attraverso la destinazione
				else if (sources.ContainsKey(inputLink))
				{
					return inputLink;
				}

				// Naviga attraverso la sorgente
				else if (destinations.ContainsKey(inputLink))
				{
					if (destinations[inputLink]._inputLink.Count > 0)
						inputLink = destinations[inputLink]._inputLink[0];
					else
						inputLink = null;
				}

				// Naviga attraverso il nodo filter
				else if (filters.ContainsKey(inputLink))
				{
					if (filters[inputLink]._outputLink.Count > 0)
						inputLink = filters[inputLink]._inputLink[0];
					else
						inputLink = null;
				}

				// Naviga attraverso l'aggregate
				else if (aggregates.ContainsKey(inputLink))
				{
					if (aggregates[inputLink]._inputLink.Count > 0)
						inputLink = aggregates[inputLink]._inputLink[0];
					else
						inputLink = null;
				}

				// Naviga attraverso il trigger events
				else if (triggerEvents.ContainsKey(inputLink))
				{
					inputLink = null;
				}

				// Naviga attraverso il trigger events
				else if (triggerActions.ContainsKey(inputLink))
				{
					return inputLink;
				}

				// Ogni altra condizione
				else
				{
					inputLink = null;
				}
			}
		}
	}

    public class Source
	{
		public class Condition
		{
			public string _attribute;
			public string _operator;
			public string _value;
		}

		public string _id;
		public string _name;
		public string _object;
		public string _table;

		public List<Condition> _conditions = new List<Condition>();

		public List<string> _outputLink = new List<string>();

		public List<string> channels = new List<string>();

		public string _triggerEvent;
		public string _triggerAction;
	}

    public class Destination
	{
		public class Condition
		{
			public string _attribute;
			public string _operator;
			public string _value;
		}

		public string _id;
		public string _name;
		public string _object;

		public List<Condition> _conditions = new List<Condition>();

		public List<string> _inputLink = new List<string>();

		public List<string> scratches = new List<string>();
	}

    public class Aggregate
	{
		public class Setting
		{
            public string _aggregate_data_name;
			public string _time_interval;
			public string _time_unit;
		}
		public class Condition
		{
			public string _typec;
			public string _namec;
			public string _unitc;
			public string _selectedc;
			public string _enabledc;
		}
		public string _id;
		public string _name;
		public string _object;
		public string _nds;

		public List<Condition> _conditions = new List<Condition>();
		public Setting _settings = new Setting();
		public List<string> _inputLink = new List<string>();
		public List<string> _outputLink = new List<string>();

		public string _destination;
		public string _source;

		public string _triggerEvent;
		public string _triggerAction;
	}

    public class Filter
	{
		public class Condition
		{
			public string _attribute;
			public string _operator;
			public string _value;
			public string _value1;
			public string _value2;
		}

		public string _id;
		public string _name;
		public string _object;

		public List<Condition> _conditions = new List<Condition>();
		public List<Condition> _orconditions = new List<Condition>();
		public List<string> _inputLink = new List<string>();
		public List<string> _outputLink = new List<string>();

		public string _destination;
		public string _source;

		public string _triggerEvent;
		public string _triggerAction;
	}

    public class TriggerAction
	{
		public string _id;
		public string _name;
		public string _object;
		public string _event;
		public string _event_state;

		public List<string> _inputLink = new List<string>();
		public List<string> _outputLink = new List<string>();

		public string _destination;
		public string _source;
	}

    public class TriggerEvent
	{
		public class Condition
		{
			public string _attribute;
			public string _operator;
			public string _value;
			public string _value1;
			public string _value2;
		}

		public class TriggerCondition
		{
			public string _expression;
			public string _data_value;
		}

		public class TriggerOn
		{
			public string _trigger_interval;
			public TriggerCondition _triggerCondition = new TriggerCondition();
			public List<Condition> _conditions = new List<Condition>();
			public List<Condition> _orconditions = new List<Condition>();
			public List<string> _outputLink = new List<string>();

			public string _destination;
		}

		public class TriggerOff
		{
			public string _trigger_interval;
			public TriggerCondition _triggerCondition = new TriggerCondition();
			public List<Condition> _conditions = new List<Condition>();
			public List<Condition> _orconditions = new List<Condition>();
			public List<string> _outputLink = new List<string>();

			public string _destination;
		}

		public string _id;
		public string _name;
		public string _object;

		public TriggerOn _triggerOn = new TriggerOn();
		public TriggerOff _triggerOff = new TriggerOff();

		public List<string> _inputLink = new List<string>();

		public string _source;
	}

	public class Join
	{
		public class Condition
		{
			public string _property;
			public string _property2;
			public string[] _id0;
			public string[] _id1;
			public string _id00;
			public string _id11;
			public string _ids;
		}
		public string _id;
		public string _name;
		public string _object;
		public string _join_type;

		public List<Condition> _join_schema = new List<Condition>();

		public List<string> _inputLink = new List<string>();
		public List<string> _outputLink = new List<string>();

		public string _destination;
		public string _source;

		public string _triggerEvent;
		public string _triggerAction;
	}

	public class Edge
	{
		public string _id;
		public string _source;
		public string _target;
	}
}
 