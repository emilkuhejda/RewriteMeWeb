import { UserSubscription } from '../_models/user-subscription';
import { SubscriptionOperation } from '../_enums/subscription-operation';

export class UserSubscriptionMapper {
    public static convertAll(data): UserSubscription[] {
        let userSubscriptions = [];
        for (let item of data) {
            let userSubscription = UserSubscriptionMapper.convert(item);
            userSubscriptions.push(userSubscription);
        }

        return userSubscriptions;
    }

    public static convert(data): UserSubscription {
        if (data === null || data === undefined)
            return null;

        let userSubscription = new UserSubscription();
        userSubscription.id = data.id;
        userSubscription.userId = data.userId;
        userSubscription.applicationId = data.applicationId;
        userSubscription.time = data.time;
        userSubscription.operation = <SubscriptionOperation>data.operation;
        userSubscription.dateCreated = new Date(data.dateCreatedUtc);

        return userSubscription;
    }
}
