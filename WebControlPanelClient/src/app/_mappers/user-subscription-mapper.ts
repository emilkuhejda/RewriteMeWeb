import { UserSubscription } from '../_models/user-subscription';

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
        let userSubscription = new UserSubscription();
        userSubscription.id = data.id;
        userSubscription.userId = data.userId;
        userSubscription.applicationId = data.applicationId;
        userSubscription.time = data.time;
        userSubscription.dateCreated = data.dateCreated;

        return userSubscription;
    }
}
