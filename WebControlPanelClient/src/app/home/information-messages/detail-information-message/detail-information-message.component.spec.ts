import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetailInformationMessageComponent } from './detail-information-message.component';

describe('DetailInformationMessageComponent', () => {
  let component: DetailInformationMessageComponent;
  let fixture: ComponentFixture<DetailInformationMessageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetailInformationMessageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetailInformationMessageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
