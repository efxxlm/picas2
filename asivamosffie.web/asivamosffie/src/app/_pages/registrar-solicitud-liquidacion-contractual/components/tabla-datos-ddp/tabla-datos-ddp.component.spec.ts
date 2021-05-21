import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaDatosDdpComponent } from './tabla-datos-ddp.component';

describe('TablaDatosDdpComponent', () => {
  let component: TablaDatosDdpComponent;
  let fixture: ComponentFixture<TablaDatosDdpComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaDatosDdpComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaDatosDdpComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
