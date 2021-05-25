import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-detalle-recursos-comprometidos',
  templateUrl: './detalle-recursos-comprometidos.component.html',
  styleUrls: ['./detalle-recursos-comprometidos.component.scss']
})
export class DetalleRecursosComprometidosComponent implements OnInit {
  
  @Input() contratacionProyecto: any[] = [];
  
  constructor() { }

  ngOnInit(): void {
  }

}
