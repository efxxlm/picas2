import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { FichaProyectoService } from 'src/app/core/_services/fichaProyecto/ficha-proyecto.service';

@Component({
  selector: 'app-entrega',
  templateUrl: './entrega.component.html',
  styleUrls: ['./entrega.component.scss']
})
export class EntregaComponent implements OnInit {

  proyectoId: number;
  dataEntrega: any = null;

  constructor(
    private fichaProyectoService: FichaProyectoService,
    private route: ActivatedRoute
  ) {
    this.route.params.subscribe((params: Params) => {
      this.proyectoId = params.id;
    });
  }

  ngOnInit(): void {
  }

  downloadPDF() {
    setTimeout(() => {
      document.title='Entrega '+this.dataEntrega?.infoProyecto?.llaveMen;
      window.print();
    }, 300);
    window.onafterprint = function(){
      window.location.reload();
    }
  }

}
