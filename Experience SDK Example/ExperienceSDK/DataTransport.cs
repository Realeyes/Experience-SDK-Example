using Amazon.Kinesis;
using Amazon.Kinesis.Model;
using Experience_SDK_Example.ExperienceSDK.Events;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Experience_SDK_Example.ExperienceSDK
{
    /// <summary>
    /// Wraps AWS.SDK kinesis communications and optimize event sending from performance point of view (manage sending batches)
    /// Batches can be collected per different conditions. for example: batch of events per second, per each 100 messages etc
    /// in this example for simplification purpose, we are using batches of 100 messages only 
    /// </summary>
    internal class DataTransport
    {
        int _batch = 100; //default batch size 

        List<BaseEvent> _queue = new List<BaseEvent>();
        AmazonKinesisClient _kinesisClient;

        /// <summary>
        /// Internal constructor
        /// </summary>
        /// <param name="_region">Region for data ingestion</param>
        internal DataTransport(Amazon.RegionEndpoint _region)
        {
            _kinesisClient =  new AmazonKinesisClient(_region);            
        }

        /// <summary>
        /// Transfers data to Realeyes services via AWS Kinesis
        /// </summary>
        /// <param name="ev">Data object to transfer</param>
        /// <param name="force">Signals to send messages immediatelly without queueing</param>
        /// <returns>Returns 'true' if data has been sent successfully </returns>
        internal bool Send(BaseEvent ev, bool force = false)
        {
            _queue.Add(ev);
            Console.WriteLine($"Event {ev.Type} has been queued");
            if (force || _queue.Count >= _batch)
            {
                Console.WriteLine($"Start sending the queue");
                var res = SendEvents(_queue);
                _queue.Clear();
                return res;
            }

            return true;
        }

        /// <summary>
        /// Transfers data to Realeyes services via AWS Kinesis
        /// </summary>
        /// <param name="events"></param>
        /// <returns></returns>
        private bool SendEvents(List<BaseEvent> events)
        {
            var result = true;
            var batch = new PutRecordsRequest();
            batch.StreamName = "live-outside-data-ingestion";
            foreach (var ev in events)
            {
                var entity = new PutRecordsRequestEntry();
                entity.PartitionKey = $"{ev.AccountHash}-{ev.ParticipantId}";
                string dataAsJson = JsonConvert.SerializeObject(ev);
                byte[] dataAsBytes = Encoding.UTF8.GetBytes(dataAsJson);
                entity.Data = new MemoryStream(dataAsBytes);
                batch.Records.Add(entity);
            }

            try
            {
                var response = _kinesisClient.PutRecordsAsync(batch).Result;
                Console.WriteLine("Successfully sent to Kinesis");
                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to send record: '{0}'", ex.Message);
                result = false;
            }
            finally
            {
                batch.Records.ForEach(r => r.Data?.Dispose());
            }

            return result;
        }
    }
}
