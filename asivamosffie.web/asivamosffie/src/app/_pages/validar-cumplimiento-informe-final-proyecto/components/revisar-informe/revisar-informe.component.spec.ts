import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RevisarInformeComponent } from './revisar-informe.component';

describe('RevisarInformeComponent', () => {
  let component: RevisarInformeComponent;
  let fixture: ComponentFixture<RevisarInformeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RevisarInformeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RevisarInformeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
