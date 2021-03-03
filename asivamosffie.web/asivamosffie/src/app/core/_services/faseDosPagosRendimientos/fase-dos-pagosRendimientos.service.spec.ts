import { TestBed } from '@angular/core/testing'

import { FaseDosPagosRendimientosService } from './fase-dos-pagosRendimientos.service'

describe('FaseDosPagosRendimientosService', () => {
  let service: FaseDosPagosRendimientosService

  beforeEach(() => {
    TestBed.configureTestingModule({})
    service = TestBed.inject(FaseDosPagosRendimientosService)
  })

  it('should be created', () => {
    expect(service).toBeTruthy()
  })
})
