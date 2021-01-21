import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FlujoIntervencionRecursosComponent } from './flujo-intervencion-recursos.component';

describe('FlujoIntervencionRecursosComponent', () => {
  let component: FlujoIntervencionRecursosComponent;
  let fixture: ComponentFixture<FlujoIntervencionRecursosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FlujoIntervencionRecursosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FlujoIntervencionRecursosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
