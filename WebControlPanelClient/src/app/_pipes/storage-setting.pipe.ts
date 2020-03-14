import { Pipe, PipeTransform } from '@angular/core';
import { StorageSetting } from '../_enums/storage-setting';

@Pipe({
    name: 'storageSetting'
})
export class StorageSettingPipe implements PipeTransform {
    transform(value: any, ...args: any[]): any {
        value = StorageSetting[value].toString();
        if (value == StorageSetting.Disk)
            return "Disk";

        if (value == StorageSetting.Database)
            return "Database"

        if (value == StorageSetting.Azure)
            return "Cloud";

        return "";
    }
}
