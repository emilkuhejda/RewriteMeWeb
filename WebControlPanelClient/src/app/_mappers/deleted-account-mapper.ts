import { DeletedAccount } from '../_models/deleted-account';

export class DeletedAccountMapper {
    public static convertAll(data): DeletedAccount[] {
        let deletedAccounts = [];
        for (let item of data) {
            let deletedAccount = DeletedAccountMapper.convert(item);
            deletedAccounts.push(deletedAccount);
        }

        return deletedAccounts;
    }

    public static convert(data): DeletedAccount {
        if (data === null || data === undefined)
            return null;

        let deletedAccount = new DeletedAccount();
        deletedAccount.id = data.id;
        deletedAccount.userId = data.userId;
        deletedAccount.dateDeleted = new Date(data.dateDeleted);

        return deletedAccount;
    }
}
