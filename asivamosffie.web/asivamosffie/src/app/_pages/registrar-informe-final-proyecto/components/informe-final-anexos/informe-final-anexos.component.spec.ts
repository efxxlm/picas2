import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InformeFinalAnexosComponent } from './informe-final-anexos.component';

describe('InformeFinalAnexosComponent', () => {
  let component: InformeFinalAnexosComponent;
  let fixture: ComponentFixture<InformeFinalAnexosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InformeFinalAnexosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InformeFinalAnexosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
