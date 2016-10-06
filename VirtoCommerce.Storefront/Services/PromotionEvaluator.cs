﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.AutoRestClients.MarketingModuleApi;
using VirtoCommerce.Storefront.Converters;
using VirtoCommerce.Storefront.Model.Marketing;
using VirtoCommerce.Storefront.Model.Marketing.Services;
using marketingModel = VirtoCommerce.Storefront.AutoRestClients.MarketingModuleApi.Models;

namespace VirtoCommerce.Storefront.Services
{
    public class PromotionEvaluator : IPromotionEvaluator
    {
        private readonly IMarketingModuleApiClient _marketingApi;

        public PromotionEvaluator(IMarketingModuleApiClient marketingApi)
        {
            _marketingApi = marketingApi;
        }

        #region IPromotionEvaluator Members

        public virtual async Task EvaluateDiscountsAsync(PromotionEvaluationContext context, IEnumerable<IDiscountable> owners)
        {
            var rewards = await _marketingApi.MarketingModulePromotion.EvaluatePromotionsAsync(context.ToPromotionEvaluationContextDTO());
            InnerEvaluateDiscounts(rewards, owners);
        }

        public virtual void EvaluateDiscounts(PromotionEvaluationContext context, IEnumerable<IDiscountable> owners)
        {
            var rewards = _marketingApi.MarketingModulePromotion.EvaluatePromotions(context.ToPromotionEvaluationContextDTO());
            InnerEvaluateDiscounts(rewards, owners);
        }

        #endregion

        protected virtual void InnerEvaluateDiscounts(IList<marketingModel.PromotionReward> rewards, IEnumerable<IDiscountable> owners)
        {
            if (rewards != null)
            {
                foreach (var owner in owners)
                {
                    owner.ApplyRewards(rewards.Select(r => r.ToPromotionReward(owner.Currency)));
                }
            }
        }
    }
}
