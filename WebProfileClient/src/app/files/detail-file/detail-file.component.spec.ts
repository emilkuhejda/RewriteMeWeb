import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetailFileComponent } from './detail-file.component';

describe('DetailFileComponent', () => {
  let component: DetailFileComponent;
  let fixture: ComponentFixture<DetailFileComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetailFileComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetailFileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
