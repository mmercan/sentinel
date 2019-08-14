
import { Observable } from 'rxjs';

export interface CommonDataStoreInterface<T> {
    dataset: Observable<T[]>;
    getAll(): Observable<T[]>;
    get(id: number | string);
    create(item: T);
    update(item: T);
    remove(id: number | string);
}
