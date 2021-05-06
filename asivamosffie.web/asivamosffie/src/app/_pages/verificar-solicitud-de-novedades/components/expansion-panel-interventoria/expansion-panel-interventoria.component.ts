import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { NovedadContractual } from 'src/app/_interfaces/novedadContractual';

@Component({
  selector: 'app-expansion-panel-interventoria',
  templateUrl: './expansion-panel-interventoria.component.html',
  styleUrls: ['./expansion-panel-interventoria.component.scss']
})
export class ExpansionPanelInterventoriaComponent implements OnInit {
  @Input() proyecto:any;
  @Input() contrato:any;
  @Input() novedad:NovedadContractual;

  @Output() guardar = new EventEmitter();

  constructor() { }

  ngOnInit(): void {
  }

  cargarRegistro(){
    this.guardar.emit(true);
  }

  validarRegistroCompletoBasico(){
    if ( this.novedad === undefined ){
      return null;
    }else{
      if (
          this.novedad.registroCompletoInformacionBasica === undefined && 
          this.novedad.registroCompletoDescripcion === undefined){
        return null
      }else if (
                this.novedad.registroCompletoInformacionBasica === false || 
                this.novedad.registroCompletoInformacionBasica === undefined ||
                this.novedad.registroCompletoDescripcion === false ||
                this.novedad.registroCompletoDescripcion === undefined){
        return false;
      }else{
        return true;
      }
    }
  }

}
