import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ResumenPComponent } from './resumen-p.component';

describe('ResumenPComponent', () => {
  let component: ResumenPComponent;
  let fixture: ComponentFixture<ResumenPComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ResumenPComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ResumenPComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
