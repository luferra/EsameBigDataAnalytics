// Created by StreamLoader on 02/11/2016 14:15:16;
package spark_translate;
import org.apache.spark.sql.SparkSession;
import org.apache.spark.sql.Column;
import org.apache.spark.sql.Dataset;
import org.apache.spark.sql.Row;
import static org.apache.spark.sql.functions.col;
import org.apache.spark.sql.functions;
import org.apache.spark.sql.functions.*;
import org.apache.spark.sql.types.StructType;
import org.apache.spark.sql.catalyst.expressions.aggregate.Average;
import org.apache.spark.sql.catalyst.expressions.aggregate.First;


public class spark_script {
	public static void main(String[] args) throws Exception {
	SparkSession spark = SparkSession
	.builder()
	.appName("Java Spark Translator")
	.master("local")
	.getOrCreate();

	Dataset<Row> s1 = spark.read().json("rain.json");

	Dataset<Row> s2 = spark.read().json("twitter.json");

	Dataset<Row> s4 = spark.read().json("rain.json");

	Dataset<Row> f2 = s1.filter((col("city_name").contains("mila")));


	if(f2.filter((col("rainfall").lt("12")).and(col("altitude").contains("pippo"))).count() < 200)
	{

	Dataset<Row> f1 = s2.filter((col("latitude").lt("aa")).and(col("id_str").contains("as2")));

	}

	else if(f2.filter((col("latitude").equalTo("333"))).count() = 12)

	{

	Dataset<Row> f3 = s4.filter((col("latitude").leq("125")).or(col("longitude").geq("54")));

	}

	}
}
