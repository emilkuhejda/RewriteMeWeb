import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetailAdministratorComponent } from './detail-administrator.component';

describe('DetailAdministratorComponent', () => {
  let component: DetailAdministratorComponent;
  let fixture: ComponentFixture<DetailAdministratorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetailAdministratorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetailAdministratorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
