import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-registrar-pagos-rendimientos',
  templateUrl: './registrar-pagos-rendimientos.component.html',
  styleUrls: ['./registrar-pagos-rendimientos.component.scss']
})
export class RegistrarPagosRendimientosComponent implements OnInit {
  verAyuda = false;
  registrarPagos = false;
  registrarRendimientos = false;
  constructor() { }

  ngOnInit(): void {
  }

}
