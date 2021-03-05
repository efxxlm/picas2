import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-registrar-nueva-actuacion-reclamacion',
  templateUrl: './registrar-nueva-actuacion-reclamacion.component.html',
  styleUrls: ['./registrar-nueva-actuacion-reclamacion.component.scss']
})
export class RegistrarNuevaActuacionReclamacionComponent implements OnInit {
  public numReclamacion = localStorage.getItem("numReclamacion");
  constructor() { }

  ngOnInit(): void {
  }

}
