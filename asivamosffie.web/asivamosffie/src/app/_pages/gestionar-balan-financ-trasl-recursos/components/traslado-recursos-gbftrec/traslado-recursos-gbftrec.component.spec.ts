import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TrasladoRecursosGbftrecComponent } from './traslado-recursos-gbftrec.component';

describe('TrasladoRecursosGbftrecComponent', () => {
  let component: TrasladoRecursosGbftrecComponent;
  let fixture: ComponentFixture<TrasladoRecursosGbftrecComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TrasladoRecursosGbftrecComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TrasladoRecursosGbftrecComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
