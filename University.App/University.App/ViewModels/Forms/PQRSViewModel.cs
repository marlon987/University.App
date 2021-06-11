using System;
using System.Collections.Generic;
using System.Text;

namespace University.App.ViewModels.Forms
{
    public class PQRSViewModel : BaseViewModel
    {
        #region Attributes
        public class TypeRequest
        {
            public string Name { get; set; }
        }

        public class RateOurService
        {
            public string Rate { get; set; }
        }

        private List<TypeRequest> _typeRequests;
        private List<RateOurService> _rateOurServices;
        #endregion

        #region Properties
        public List<TypeRequest> TypeRequests
        {
            get { return this._typeRequests; }
            set { this.SetValue(ref this._typeRequests, value); }
        }

        public List<RateOurService> RateOurServices
        {
            get { return this._rateOurServices; }
            set { this.SetValue(ref this._rateOurServices, value); }
        }
        #endregion

        #region Constructor
        public PQRSViewModel()
        {
            this.LoadTypeRequests();
            this.LoadRateOurServices();
        }
        #endregion

        #region Methods
        private void LoadTypeRequests()
        {
            this.TypeRequests = new List<TypeRequest>
            {
                new TypeRequest {Name ="Petition"},
                new TypeRequest {Name ="Complain"},
                new TypeRequest {Name ="Claim"},
                new TypeRequest {Name ="Suggestion"}
            };
        }

        private void LoadRateOurServices()
        {
            this.RateOurServices = new List<RateOurService>
            {
                new RateOurService {Rate ="Bad"},
                new RateOurService {Rate ="Regular"},
                new RateOurService {Rate ="Well"},
                new RateOurService {Rate ="Acceptable"},
                new RateOurService {Rate ="Excellent"}
            };
        }
        #endregion
    }
}
